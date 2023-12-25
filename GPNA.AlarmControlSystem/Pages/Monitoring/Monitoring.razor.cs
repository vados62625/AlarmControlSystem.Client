using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.ImitatedAlarm;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.KpiSettings;
using GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace GPNA.AlarmControlSystem.Pages.Monitoring;

public partial class Monitoring : ComponentBase
{
    [Inject] private IOptions<AcsModuleOptions>? Options { get; set; }
    [Inject] private IIncomingAlarmService? IncomingAlarmService { get; set; }
    [Inject] private IActiveAlarmService? ActiveAlarmService { get; set; }

    [Inject] private IImitatedAlarmService? ImitatedAlarmService { get; set; }
    [Inject] private ISuppressedAlarmService? SuppressedAlarmService { get; set; }
    [Inject] private IFieldService? FieldService { get; set; }
    [Inject] private IWorkStationService? WorkStationService { get; set; }
    [Inject] private IExportService? ExportService { get; set; }
    [Inject] private ISpinnerService SpinnerService { get; set; } = default!;

    [Inject] private MonitoringSettingsService? MonitoringSettingsService { get; set; }
    [Parameter] [SupplyParameterFromQuery] public int? WorkstationId { get; set; }
    [Parameter] [SupplyParameterFromQuery] public int? FieldId { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public int AlarmType
    {
        get => _alarmType;
        set
        {
            _alarmType = value;
            _currentPage = 1;
        }
    }

    private int _alarmType = 1;
    
    
    private string? _workstationName, _fieldName;

    private WorkStationDto[]? _workstations;
    private FieldDto[]? _fields;

    private DateTimeOffset _from, _to;

    private int _pagesCount, _totalCount;
    private int _currentPage = 1;

    private string _tagNameFilter = string.Empty;
    private string? _fireStatus, _gasStatus;

    private Dictionary<StateType, bool>? _stateFilter;
    private Dictionary<PriorityType, bool>? _priorityFilter;

    private bool FiltersOn => !string.IsNullOrWhiteSpace(_tagNameFilter) || _stateFilter != default ||
                              _priorityFilter != default;

    private AlarmsCollection<IncomingAlarmDto[]>? _incomingAlarmsCollection;
    private AlarmsCollection<ActiveAlarmDto>? _activeAlarmsCollection;
    private AlarmsCollection<SuppressedAlarmDto>? _suppressedAlarmsCollection;
    private AlarmsCollection<ImitatedAlarmDto>? _imitatedAlarmsCollection;

    private IDictionary<string, string>? _fieldLinksDictionary;

    private IDictionary<string, string>? _workstationLinksDictionary;

    private Dictionary<PriorityType, int>? _countByPriority;

    private Dictionary<StateType, int>? _countByState;

    private string _spinnerClass = string.Empty;

    private string _orderBy = string.Empty;

    private bool _orderByDesc = true;

    private Dictionary<AlarmTypeEnum, int> _tabAlarmsCount = new();

    private MonitoringSettingsDto? _monitoringSettings;
    
    protected override void OnInitialized()
    {
        SetDates();
        InitFilters();
    }

    protected override async Task OnParametersSetAsync()
    {
        await SetFieldWithWorkstation();
        _monitoringSettings = await GetSettings();
        await InitializePageAsync();
    }

    private async Task SetFieldWithWorkstation()
    {
        if (FieldService != null)
        {
            var fields = await FieldService.GetList();
            if (fields.Success)
                _fields = fields.Payload.ToArray();
        }

        if (WorkStationService != null)
        {
            var workstations = await WorkStationService.GetList(new { FieldId = FieldId });
            if (workstations.Success)
                _workstations = workstations.Payload.ToArray();
        }

        FieldId ??= _fields?.FirstOrDefault()?.Id;
        _fieldName = _fields?.FirstOrDefault(field => field.Id == FieldId)?.Name;

        WorkstationId ??= _workstations?.FirstOrDefault()?.Id;
        _workstationName = _workstations?.FirstOrDefault(ws => ws.Id == WorkstationId)?.Name;

        StateHasChanged();

        FillLinks();
    }

    private void SetAlarmsCounts<T>(AlarmsCollection<T> alarms)
    {
        _totalCount = alarms.TotalCount;
        _pagesCount = alarms.PagesCount;
        _fireStatus = alarms.Fire ? "active" : "";
        _gasStatus = alarms.Gas ? "active" : "";
        _countByPriority = alarms.CountByPriority;
        _countByState = alarms.CountByState;


        _tabAlarmsCount[AlarmTypeEnum.Active] = alarms.ActiveAlarmsCount;
        _tabAlarmsCount[AlarmTypeEnum.Incoming] = alarms.IncomingAlarmsCount;
        _tabAlarmsCount[AlarmTypeEnum.Simulation] = alarms.ImitationParamsCount;
        _tabAlarmsCount[AlarmTypeEnum.Suppressed] = alarms.SuppressedAlarmsCount;
    }

    private void FillLinks()
    {
        if (_fields != null)
        {
            _fieldLinksDictionary = _fields.ToDictionary(field =>
                    field.Name,
                field => $"/monitoring/?alarmType={(int)AlarmType}&fieldId={field.Id}");
        }

        if (_workstations != null)
        {
            _workstationLinksDictionary = _workstations.ToDictionary(workStation =>
                    workStation.Name ?? Guid.NewGuid().ToString(),
                workStation =>
                    $"/monitoring/?alarmType={(int)AlarmType}&fieldId={FieldId}&workstationId={workStation.Id}");
        }
    }

    private void InitFilters()
    {
        _stateFilter = Enum.GetValues<StateType>().ToDictionary(c => c, _ => true);
        _priorityFilter = Enum.GetValues<PriorityType>().ToDictionary(c => c, _ => true);

        _priorityFilter[PriorityType.Information] = false;
    }

    private async Task InitializePageAsync()
    {
        switch ((AlarmTypeEnum)AlarmType)
        {
            case AlarmTypeEnum.Incoming:
            {
                await SpinnerService.Load(UpdateIncomingAlarms);
                break;
            }
            case AlarmTypeEnum.Active:
            {
                await SpinnerService.Load(UpdateActiveAlarms);
                break;
            }
            case AlarmTypeEnum.Simulation:
            {
                await SpinnerService.Load(UpdateImitatedAlarms);
                break;
            }
            case AlarmTypeEnum.Suppressed:
            {
                await SpinnerService.Load(UpdateSuppressedAlarms);
                break;
            }
        }
    }

    private void SetDates()
    {
        _to = DateTimeOffset.Now;
        var hourOfDay = _to.Hour is > 8 and < 20 ? 8 : 20; // 8 20 часов. Выставление времени с Начала смены.
        var day = _to.Hour < 8 ? _to.Day - 1 : _to.Day;
        _from = new DateTimeOffset(_to.Year, _to.Month, day, hourOfDay, 0, 0, 0, _to.Offset);
    }

    private async Task<MonitoringSettingsDto?> GetSettings()
    {
        if (MonitoringSettingsService == default || WorkstationId is null) return null;
        
        var result = await MonitoringSettingsService.GetSettings(WorkstationId.Value);

        return result.Success ? result.Payload : null;
    }
    
    private async Task UpdateIncomingAlarms()
    {
        if (IncomingAlarmService != null)
        {
            var request = await IncomingAlarmService.GetAlarmsPerDate(new GetIncomingAlarmsByDatesQuery
            {
                WorkStationId = WorkstationId ?? 0,
                Suggest = _tagNameFilter,
                DateTimeStart = _from,
                DateTimeEnd = _to,
                State = _stateFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
                Priority = _priorityFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
                OrderPropertyName = _orderBy,
                OrderByDescending = _orderByDesc,
                Page = _currentPage,
                CountOnPage = 15,
                DisplayShifts = !FiltersOn &&
                                (_orderBy == nameof(IncomingAlarmDto.DateTimeStart) || _orderBy == string.Empty)
            });

            if (request.Success)
            {
                _incomingAlarmsCollection = request.Payload;
                _pagesCount = _incomingAlarmsCollection.PagesCount;
            }
        }

        if (_incomingAlarmsCollection != null) SetAlarmsCounts(_incomingAlarmsCollection);
    }

    private async Task UpdateActiveAlarms()
    {
        if (ActiveAlarmService != null)
        {
            var request = await ActiveAlarmService.GetActiveAlarmsPerDate(new GetIncomingAlarmsByDatesQuery
            {
                WorkStationId = WorkstationId ?? 0,
                Suggest = _tagNameFilter,
                DateTimeStart = _from,
                DateTimeEnd = _to,
                State = _stateFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
                Priority = _priorityFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
                OrderPropertyName = _orderBy,
                OrderByDescending = _orderByDesc,
                Page = _currentPage,
                CountOnPage = 15,
                DisplayShifts = false
                // DisplayShifts = !FiltersOn && (_orderBy == nameof(IncomingAlarmDto.DateTimeActivation) || _orderBy == string.Empty)
            });

            if (request.Success)
            {
                _activeAlarmsCollection = request.Payload;
                _pagesCount = _activeAlarmsCollection.PagesCount;
            }
        }

        if (_activeAlarmsCollection != null) SetAlarmsCounts(_activeAlarmsCollection);
    }

    private async Task UpdateImitatedAlarms()
    {
        if (ImitatedAlarmService != null)
        {
            var request = await ImitatedAlarmService.GetImitatedAlarmsPerDate(new GetIncomingAlarmsByDatesQuery
            {
                WorkStationId = WorkstationId ?? 0,
                Suggest = _tagNameFilter,
                DateTimeStart = _from,
                DateTimeEnd = _to,
                State = _stateFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
                Priority = _priorityFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
                OrderPropertyName = _orderBy,
                OrderByDescending = _orderByDesc,
                Page = _currentPage,
                CountOnPage = 15,
                DisplayShifts = false
            });

            if (request.Success)
            {
                _imitatedAlarmsCollection = request.Payload;
                _pagesCount = _imitatedAlarmsCollection.PagesCount;
            }
        }

        if (_imitatedAlarmsCollection != null) SetAlarmsCounts(_imitatedAlarmsCollection);
    }

    private async Task UpdateSuppressedAlarms()
    {
        if (SuppressedAlarmService != null)
        {
            var request = await SuppressedAlarmService.GetSuppressedAlarmsPerDate(new GetIncomingAlarmsByDatesQuery
            {
                WorkStationId = WorkstationId ?? 0,
                Suggest = _tagNameFilter,
                DateTimeStart = _from,
                DateTimeEnd = _to,
                State = _stateFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
                Priority = _priorityFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
                OrderPropertyName = _orderBy,
                OrderByDescending = _orderByDesc,
                Page = _currentPage,
                CountOnPage = 15,
                DisplayShifts = false
                // DisplayShifts = !FiltersOn && (_orderBy == nameof(IncomingAlarmDto.DateTimeActivation) || _orderBy == string.Empty)
            });

            if (request.Success)
            {
                _suppressedAlarmsCollection = request.Payload;
                _pagesCount = _suppressedAlarmsCollection.PagesCount;
            }
        }

        if (_suppressedAlarmsCollection != null) SetAlarmsCounts(_suppressedAlarmsCollection);
    }

    private Task DropFilters()
    {
        _tagNameFilter = string.Empty;

        _currentPage = 1;

        foreach (var state in Enum.GetValues<StateType>())
        {
            _stateFilter[state] = true;
        }

        foreach (var priority in Enum.GetValues<PriorityType>())
        {
            _priorityFilter[priority] = true;
        }

        return InitializePageAsync();
    }

    private async Task<Stream?> GetIncomingExportStream()
    {
        var result = await ExportService.ExportIncomingAlarms(new ExportIncomingAlarmsByDatesQuery
        {
            DocumentType = ExportDocumentType.Excel,
            WorkStationId = WorkstationId ?? 0,
            Suggest = _tagNameFilter,
            DateTimeStart = _from,
            DateTimeEnd = _to,
            State = _stateFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
            Priority = _priorityFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
            CountOnPage = int.MaxValue
        });

        return new MemoryStream(result);
    }

    private async Task<Stream?> GetActiveExportStream()
    {
        var result = await ExportService.ExportActiveAlarms(new ExportIncomingAlarmsByDatesQuery
        {
            DocumentType = ExportDocumentType.Excel,
            WorkStationId = WorkstationId ?? 0,
            Suggest = _tagNameFilter,
            DateTimeStart = _from,
            DateTimeEnd = _to,
            State = _stateFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
            Priority = _priorityFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
            CountOnPage = int.MaxValue
        });

        return new MemoryStream(result);
    }

    private async Task<Stream?> GetImitatedExportStream()
    {
        var result = await ExportService.ExportImitatedAlarms(new ExportIncomingAlarmsByDatesQuery
        {
            DocumentType = ExportDocumentType.Excel,
            WorkStationId = WorkstationId ?? 0,
            Suggest = _tagNameFilter,
            DateTimeStart = _from,
            DateTimeEnd = _to,
            State = _stateFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
            Priority = _priorityFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
            CountOnPage = int.MaxValue
        });

        return new MemoryStream(result);
    }

    private async Task<Stream?> GetSuppressedExportStream()
    {
        var result = await ExportService.ExportSuppressedAlarms(new ExportIncomingAlarmsByDatesQuery
        {
            DocumentType = ExportDocumentType.Excel,
            WorkStationId = WorkstationId ?? 0,
            Suggest = _tagNameFilter,
            DateTimeStart = _from,
            DateTimeEnd = _to,
            State = _stateFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
            Priority = _priorityFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
            CountOnPage = int.MaxValue
        });

        return new MemoryStream(result);
    }

    private async Task DownloadFileFromStream()
    {
        SpinnerService?.Show();

        var fileStream = AlarmType switch
        {
            (int)Models.Enums.AlarmTypeEnum.Incoming => await GetIncomingExportStream(),
            (int)Models.Enums.AlarmTypeEnum.Active => await GetActiveExportStream(),
            (int)Models.Enums.AlarmTypeEnum.Simulation => await GetImitatedExportStream(),
            (int)Models.Enums.AlarmTypeEnum.Suppressed => await GetSuppressedExportStream(),
            _ => default
        };

        if (fileStream == null) return;

        var fileName = "export.xlsx";

        using var streamRef = new DotNetStreamReference(stream: fileStream);

        await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

        SpinnerService?.Hide();
    }

    private async Task ToggleStateFilter(StateType? state)
    {
        if (state == default || _stateFilter == default) return;

        _stateFilter[state.Value] = !_stateFilter[state.Value];
        _currentPage = 1;

        await InitializePageAsync();
    }

    private async Task TogglePriorityFilter(PriorityType? priority)
    {
        if (priority == default || _priorityFilter == default) return;

        _priorityFilter[priority.Value] = !_priorityFilter[priority.Value];
        _currentPage = 1;

        await InitializePageAsync();
    }

    private async Task RefreshBySpinner()
    {
        _spinnerClass = " active";
        _currentPage = 1;

        await InitializePageAsync();

        _spinnerClass = string.Empty;
    }

    private async Task OnOrderingChanged(string orderBy)
    {
        if (_orderBy == orderBy) _orderByDesc = !_orderByDesc;

        _orderBy = orderBy;
        _currentPage = 1;

        await InitializePageAsync();
    }

    private async Task OnPageChanged(int page)
    {
        _currentPage = page;
        await InitializePageAsync();
    }

    private async Task Search(string tagname)
    {
        _tagNameFilter = tagname;
        _currentPage = 1;
        await InitializePageAsync();
    }
}