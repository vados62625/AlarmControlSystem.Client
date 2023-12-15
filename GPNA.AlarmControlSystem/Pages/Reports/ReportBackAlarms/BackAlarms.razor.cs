using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace GPNA.AlarmControlSystem.Pages.Reports.ReportBackAlarms;

public partial class BackAlarms : ComponentBase
{
    [Inject]
    private IOptions<AcsModuleOptions>? Options { get; set; }

    [Inject]
    private IFieldService? FieldService { get; set; }
    
    [Inject]
    private IIncomingAlarmService? IncomingAlarmService { get; set; }

    [Inject]
    private IWorkStationService? WorkStationService { get; set; }

    [Inject]
    protected ISpinnerService SpinnerService { get; set; } = default!;

    private DateTimeOffset DateTimeStart { get; } = new(DateTimeOffset.Now.AddDays(-16).Year, DateTimeOffset.Now.AddDays(-16).Month, DateTimeOffset.Now.AddDays(-16).Day, 0, 0, 0, DateTimeOffset.Now.AddDays(-16).Offset);
    private DateTimeOffset DateTimeEnd { get; } = new(DateTimeOffset.Now.AddDays(-1).Year, DateTimeOffset.Now.AddDays(-1).Month, DateTimeOffset.Now.AddDays(-1).Day, 23, 59, 59, DateTimeOffset.Now.AddDays(-1).Offset);

    [Parameter]
    [SupplyParameterFromQuery]
    public int? FieldId { get; set; }
    
    private FieldDto[]? _fields;

    private WorkStationDto[]? _workstations;

    private Dictionary<int, Dictionary<AlarmType, Dictionary<DateTime, int>>>? _expiredAlarmsCount;

    private IDictionary<string, string>? WorkstationLinksDictionary { get; set; }

    protected override async Task OnInitializedAsync()
    {
        SpinnerService.Show();

        await InitializePageAsync();

        SpinnerService.Hide();

        await base.OnInitializedAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        await GetWorkstations();
        
        await GetExpiredAlarmsCount();

        FillWorkstationLinks();
    }


    private async Task InitializePageAsync()
    {
        await GetFields();

        FieldId ??= _fields?.FirstOrDefault()?.Id ?? 1;

        await GetWorkstations();
        
        await GetExpiredAlarmsCount();

        FillWorkstationLinks();
    }

    private async Task GetFields()
    {
        if (FieldService != null)
        {
            var result = await FieldService.GetList();

            if (result.Success)
            {
                _fields = result.Payload.ToArray();
            }
        }
    }

    private async Task GetWorkstations()
    {
        if (WorkStationService != null && FieldId != null)
        {
            var query = new GetAlarmsCountForFieldQuery { FieldId = FieldId.Value, DateTimeStart = DateTimeStart, DateTimeEnd = DateTimeEnd };
            var result = await WorkStationService.GetList(query);

            if (result.Success)
            {
                _workstations = result.Payload.ToArray();
            }
        }
    }
    
    private async Task GetExpiredAlarmsCount()
    {
        if (IncomingAlarmService != null && FieldId != null)
        {
            var query = new GetExpiredAlarmsByDatesQuery { FieldId = FieldId.Value, DateTimeStart = DateTimeStart, DateTimeEnd = DateTimeEnd };
            var result = await IncomingAlarmService.GetExpiredCountInHour(query);

            if (result.Success)
            {
                _expiredAlarmsCount = result.Payload;
            }
        }
    }

    private void FillWorkstationLinks()
    {
        if (_fields != null && _fields.Any())
        {
            WorkstationLinksDictionary = _fields.ToDictionary(field => field.Name, field => $"/reports/back-alarms?fieldId={field.Id}");
        }
    }
}