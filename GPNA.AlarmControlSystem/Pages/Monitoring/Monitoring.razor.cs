using GPNA.AlarmControlSystem.Extensions;
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

public partial class Monitoring : AcsPageBase
{
    [Inject] private IOptions<AcsModuleOptions>? Options { get; set; }
    [Inject] private IIncomingAlarmService? IncomingAlarmService { get; set; }
    [Inject] private IActiveAlarmService? ActiveAlarmService { get; set; }

    [Inject] private IImitatedAlarmService? ImitatedAlarmService { get; set; }
    [Inject] private ISuppressedAlarmService? SuppressedAlarmService { get; set; }
    [Inject] private IExportService? ExportService { get; set; }

    [Inject] private MonitoringSettingsService? MonitoringSettingsService { get; set; }

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
    
    protected override async Task OnInitializedAsync()
    {
        SetDates();
        InitFilters();
        _monitoringSettings = await GetSettings();
    }

    protected override async Task LoadPage()
    {
        await InitializePageAsync();
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
            var request = await IncomingAlarmService.GetIncomingAlarmsCollection(GetAlarmsCollectionQuery());

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
            var request = await ActiveAlarmService.GetActiveAlarmsCollection(GetAlarmsCollectionQuery());

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
            var request = await ImitatedAlarmService.GetImitatedAlarmsCollection(GetAlarmsCollectionQuery());

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
            var request = await SuppressedAlarmService.GetSuppressedAlarmsCollection(GetAlarmsCollectionQuery());

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
        var result = await ExportService.ExportIncomingAlarms(GetExportAlarmsCollectionQuery());

        return new MemoryStream(result);
    }

    private async Task<Stream?> GetActiveExportStream()
    {
        var result = await ExportService.ExportActiveAlarms(GetExportAlarmsCollectionQuery());

        return new MemoryStream(result);
    }

    private async Task<Stream?> GetImitatedExportStream()
    {
        var result = await ExportService.ExportImitatedAlarms(GetExportAlarmsCollectionQuery());

        return new MemoryStream(result);
    }

    private async Task<Stream?> GetSuppressedExportStream()
    {
        var result = await ExportService.ExportSuppressedAlarms(GetExportAlarmsCollectionQuery());

        return new MemoryStream(result);
    }

    private async Task DownloadFileFromStream()
    {
        SpinnerService?.Show();

        var fileStream = (AlarmTypeEnum)AlarmType switch
        {
            AlarmTypeEnum.Incoming => await GetIncomingExportStream(),
            AlarmTypeEnum.Active => await GetActiveExportStream(),
            AlarmTypeEnum.Simulation => await GetImitatedExportStream(),
            AlarmTypeEnum.Suppressed => await GetSuppressedExportStream(),
            _ => default
        };

        if (fileStream == null) return;

        var fileName = $"{((AlarmTypeEnum)AlarmType).GetDescription()} - экспорт.xlsx";

        using var streamRef = new DotNetStreamReference(stream: fileStream);

        await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

        SpinnerService?.Hide();
    }

    private async Task ToggleStateFilter(StateType? state)
    {
        if (_stateFilter == default) return;

        if (state == null)
        {
            var enableAll = _stateFilter.Any(c => !c.Value);
            foreach (var key in _stateFilter.Keys)
            {
                _stateFilter[key] = enableAll;
            }
        }
        else
        {
            _stateFilter[state.Value] = !_stateFilter[state.Value];
            _currentPage = 1;
        }

        await InitializePageAsync();
    }

    private async Task TogglePriorityFilter(PriorityType? priority)
    {
        if (_priorityFilter == default) return;
        
        if (priority == null)
        {
            var enableAll = _priorityFilter.Any(c => !c.Value);
            foreach (var key in _priorityFilter.Keys)
            {
                _priorityFilter[key] = enableAll;
            }
        }
        else
        {
            _priorityFilter[priority.Value] = !_priorityFilter[priority.Value];
            _currentPage = 1;
        }

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

    private GetAlarmsCollectionQueryBase GetAlarmsCollectionQuery()
    {
        return new GetAlarmsCollectionQueryBase
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
            ItemsOnPage = 15,
            DisplayShifts = !FiltersOn &&
                            (AlarmTypeEnum)AlarmType == AlarmTypeEnum.Incoming &&
                            (_orderBy == nameof(IncomingAlarmDto.DateTimeStart) || _orderBy == string.Empty)
        };
    }

    private ExportAlarmsCollectionQueryBase GetExportAlarmsCollectionQuery()
    {
        return new ExportAlarmsCollectionQueryBase
        {
            DocumentType = ExportDocumentType.Excel,
            WorkStationId = WorkstationId ?? 0,
            Suggest = _tagNameFilter,
            DateTimeStart = _from,
            DateTimeEnd = _to,
            State = _stateFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
            Priority = _priorityFilter?.Where(c => c.Value).Select(c => c.Key).ToList(),
            ItemsOnPage = int.MaxValue
        };
    }
}