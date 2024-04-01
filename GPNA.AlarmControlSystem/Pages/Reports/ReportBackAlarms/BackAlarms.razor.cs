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

public partial class BackAlarms : AcsPageBase
{
    [Inject] private IOptions<AcsModuleOptions>? Options { get; set; }
    
    [Inject] private IIncomingAlarmService? IncomingAlarmService { get; set; }
    
    private DateTimeOffset DateTimeStart { get; } = new(DateTimeOffset.Now.AddDays(-16).Year,
        DateTimeOffset.Now.AddDays(-16).Month, DateTimeOffset.Now.AddDays(-16).Day, 0, 0, 0,
        DateTimeOffset.Now.AddDays(-16).Offset);

    private DateTimeOffset DateTimeEnd { get; } = new(DateTimeOffset.Now.AddDays(-1).Year,
        DateTimeOffset.Now.AddDays(-1).Month, DateTimeOffset.Now.AddDays(-1).Day, 23, 59, 59,
        DateTimeOffset.Now.AddDays(-1).Offset);

    [Parameter] [SupplyParameterFromQuery] public int? AlarmType { get; set; }

    private Dictionary<int, Dictionary<AlarmType, Dictionary<DateTime, int>>>? _expiredAlarmsCount;

    private Dictionary<AlarmType, Dictionary<DateTime, int>>? _expiredAlarmsCountChart;

    private IDictionary<string, string> AlarmTypeLinksDictionary => new Dictionary<string, string>
    {
        { "Активация", $"/reports/back-alarms?alarmType=0&fieldId={FieldId}&workstationId={WorkstationId}" },
        { "Имитация", $"/reports/back-alarms?alarmType=4&fieldId={FieldId}&workstationId={WorkstationId}" },
        { "Подавление", $"/reports/back-alarms?alarmType=2&fieldId={FieldId}&workstationId={WorkstationId}" },
    };

    private bool _isEnableRenderChart;
    private bool _notFirstRender;
    private int? _fieldId;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        // You must consume parameters immediately
        parameters.SetParameterProperties(this);

        // short circuit if we've already rendered once and the messageid is the same
        if (FieldId == _fieldId && _notFirstRender)
        {
            await ReloadChart();
            return;
        }

        _fieldId = FieldId;
        _notFirstRender = true;

        // Run the lifecycle events
        // Note passing on an empty ParameterView to the base.  We've already set them
        await base.SetParametersAsync(ParameterView.Empty);
    }
    
    protected override void FillLinks()
    {
        if (Fields != null)
        {
            FieldLinksDictionary = Fields.ToDictionary(field =>
                    field.Name,
                field => $"{GetPageFromUrl()}?fieldId={field.Id}");
        }
        
        if (Workstations != null)
        {
            WorkstationLinksDictionary = Workstations.ToDictionary(workStation =>
                    workStation.Name ?? Guid.NewGuid().ToString(),
                workStation => $"{GetPageFromUrl()}?fieldId={FieldId}&workstationId={workStation.Id}&alarmType={AlarmType}");
        }
    }

    protected override async Task LoadPageAsync()
    {
        _fieldId ??= FieldId;
        await SpinnerService.Load(GetExpiredAlarmsCount);
        await ReloadChart();
    }

    private async Task ReloadChart()
    {
        FillLinks();
        _isEnableRenderChart = true;
        
        UpdateExpiredAlarmsCountSum();
        
        StateHasChanged();

        await Task.Delay(100);
        _isEnableRenderChart = false;
    }

    private async Task GetExpiredAlarmsCount()
    {
        if (IncomingAlarmService != null && FieldId != null)
        {
            var query = new GetExpiredAlarmsByDatesQuery
                { FieldId = FieldId.Value, DateTimeStart = DateTimeStart, DateTimeEnd = DateTimeEnd };
            var result = await IncomingAlarmService.GetExpiredCountInHour(query);

            if (result.Success)
            {
                _expiredAlarmsCount = result.Payload;
            }
        }
    }

    private void UpdateExpiredAlarmsCountSum()
    {
        var expiredAlarmsCount = _expiredAlarmsCount!.AsEnumerable();
        if (_expiredAlarmsCount!.TryGetValue(WorkstationId!.Value, out _))
        {
            expiredAlarmsCount = expiredAlarmsCount.Where(c => c.Key == WorkstationId.Value);
        }
        _expiredAlarmsCountChart = expiredAlarmsCount
            .SelectMany(c => c.Value)
            .GroupBy(pair => pair.Key)
            .ToDictionary(x => x.Key, x =>
                x.ToArray()
                    .SelectMany(dict => dict.Value)
                    .GroupBy(pair => pair.Key)
                    .ToDictionary(group => group.Key, group => group.Sum(pair => pair.Value)));
    }
}