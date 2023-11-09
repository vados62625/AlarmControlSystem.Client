using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
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
    
    [Inject] private ISuppressedAlarmService? SuppressedAlarmService { get; set; }

    [Inject] private IFieldService? FieldService { get; set; }

    [Inject] private IWorkStationService? WorkStationService { get; set; }

    [Inject] private IExportService? ExportService { get; set; }

    [Inject] private ISpinnerService? SpinnerService { get; set; }

    [Parameter] public DateTime DateTime { get; set; }

    [Parameter] [SupplyParameterFromQuery] public int? ArmId { get; set; }

    [Parameter] [SupplyParameterFromQuery] public int? FieldId { get; set; }

    [Parameter] public string? ArmName { get; set; }

    [Parameter] public string? FieldName { get; set; }

    [Parameter]
    public DateTimeOffset From { get; set; }

    [Parameter]
    public DateTimeOffset To { get; set; }

    [Parameter]
    public int PagesCount { get; set; }

    [Parameter]
    public int CurrentPage { get; set; } = 1;

    [Parameter] public string TagNameFilter { get; set; } = string.Empty;

    [Parameter]
    public StateType? StateFilter { get; set; }

    [Parameter]
    public PriorityType? PriorityFilter { get; set; }

    [Parameter]
    public AlarmType AlarmType { get; set; } = AlarmType.Incoming;

    private bool FiltersOn =>
        !string.IsNullOrWhiteSpace(TagNameFilter) || StateFilter != default || PriorityFilter != default;

    private AlarmsCollection<IncomingAlarmDto>? AlarmsCollection { get; set; }

    private FieldDto[]? _fields;

    private WorkStationDto[]? _workstations;

    private IDictionary<string, string>? FieldLinksDictionary { get; set; }

    private IDictionary<string, string>? ArmLinksDictionary { get; set; }

    private string _spinnerClass = string.Empty;

    private string _orderBy = string.Empty;

    private bool _orderByDesc = true;

    protected override void OnInitialized()
    {
        SetDates();
        base.OnInitialized();
    }

    protected override async Task OnParametersSetAsync()
    {
        await SetFieldWithArm();

        await InitializePageAsync();
    }

    private async Task SetFieldWithArm()
    {
        if (FieldService != null)
        {
            var fields = await FieldService.GetList();

            if (fields.Success)
            {
                _fields = fields.Payload.ToArray();
            }
        }

        if (WorkStationService != null)
        {
            var workstations = await WorkStationService.GetList(new { FieldId });

            if (workstations.Success)
            {
                _workstations = workstations.Payload.ToArray();
            }
        }

        FieldId ??= _fields?.FirstOrDefault()?.Id;

        FieldName = _fields?.FirstOrDefault(field => field.Id == FieldId)?.Name;

        ArmId ??= _workstations?.FirstOrDefault()?.Id;

        ArmName = _workstations?.FirstOrDefault(ws => ws.Id == ArmId)?.Name;

        FillLinks();
    }

    private void FillLinks()
    {
        if (_fields != null)
        {
            FieldLinksDictionary = _fields.ToDictionary(field =>
                    field.Name,
                field => $"/Arm/?fieldId={field.Id}");
        }

        if (_workstations != null)
        {
            ArmLinksDictionary = _workstations.ToDictionary(workStation =>
                    workStation.Name ?? Guid.NewGuid().ToString(),
                workStation => $"/Arm/?fieldId={FieldId}&armId={workStation.Id}");
        }
    }

    private async Task InitializePageAsync()
    {
        SpinnerService?.Show();

        await UpdateAlarms();

        SpinnerService?.Hide();
    }

    private void SetDates()
    {
        To = DateTimeOffset.Now;
        From = new DateTimeOffset(To.Year, To.Month, To.Day, 8, 0, 0, 0, To.Offset);
    }

    private async Task UpdateAlarms()
    {
        if (IncomingAlarmService != null)
        {
            var request = await IncomingAlarmService.GetAlarmsPerDate(new GetAlarmsCollectionQuery
            {
                WorkStationId = ArmId ?? 0,
                TagName = TagNameFilter,
                ActivationFrom = From,
                ActivationTo = To,
                State = StateFilter,
                Priority = PriorityFilter,
                OrderPropertyName = _orderBy,
                OrderByDescending = _orderByDesc,
                Page = CurrentPage,
                CountOnPage = 15,
                DisplayShifts = !FiltersOn && _orderBy == nameof(IncomingAlarmDto.DateTimeActivation)
            });

            if (request.Success)
            {
                AlarmsCollection = request.Payload;
                PagesCount = AlarmsCollection.PagesCount;
            }
        }
    }

    private Task DropFilters()
    {
        TagNameFilter = string.Empty;
        StateFilter = null;
        PriorityFilter = null;
        CurrentPage = 1;

        return InitializePageAsync();
    }

    private async Task<Stream?> GetFileStream()
    {
        var result = await ExportService.ExportIncomingAlarms(new ExportAlarmsCollectionQuery
        {
            DocumentType = ExportDocumentType.Excel,
            WorkStationId = ArmId ?? 0,
            TagName = TagNameFilter,
            ActivationFrom = From,
            ActivationTo = To,
            State = StateFilter,
            Priority = PriorityFilter,
        });

        return new MemoryStream(result);
    }

    private async Task DownloadFileFromStream()
    {
        SpinnerService?.Show();

        var fileStream = await GetFileStream();

        if (fileStream == null) return;

        var fileName = "export.xlsx";

        using var streamRef = new DotNetStreamReference(stream: fileStream);

        await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

        SpinnerService?.Hide();
    }

    private async Task SetStateFilter(StateType? state)
    {
        StateFilter = state;
        CurrentPage = 1;

        await InitializePageAsync();
    }

    private async Task SetPriorityFilter(PriorityType? priority)
    {
        PriorityFilter = priority;
        CurrentPage = 1;

        await InitializePageAsync();
    }

    private async Task RefreshBySpinner()
    {
        _spinnerClass = " active";
        CurrentPage = 1;

        await InitializePageAsync();

        _spinnerClass = string.Empty;
    }

    private async Task OnOrderingChanged(string orderBy)
    {
        if (_orderBy == orderBy) _orderByDesc = !_orderByDesc;

        _orderBy = orderBy;
        CurrentPage = 1;

        await InitializePageAsync();
    }

    private async Task OnPageChanged(int page)
    {
        CurrentPage = page;
        await InitializePageAsync();
    }
}