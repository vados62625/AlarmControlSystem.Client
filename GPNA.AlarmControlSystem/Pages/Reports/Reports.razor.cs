using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace GPNA.AlarmControlSystem.Pages.Reports
{
    public partial class Reports : ComponentBase
    {
        [Inject]
        private IOptions<AcsModuleOptions>? Options { get; set; }
        
        [Inject] IBufferAlarmService AlarmService { get; set; } = null!;
        [Inject] IIncomingAlarmService IncomingAlarmService { get; set; } = null!;
        [Inject] protected ISpinnerService SpinnerService { get; set; } = default!;
        
        [Inject]
        private IFieldService? FieldService { get; set; }

        [Inject]
        private IWorkStationService? WorkStationService { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int? FieldId { get; set; }
        
        [Parameter]
        [SupplyParameterFromQuery]
        public int? ArmId { get; set; }
        
        [Parameter] public DateTimeOffset From { get; set; }
        [Parameter] public DateTimeOffset To { get; set; }
        
        [Parameter]
        public string? ArmName { get; set; }

        private string? FieldName => _fields?.FirstOrDefault(field => field.Id == FieldId)?.Name ?? "N/A";
        
        static int inputKpi = 12;
        private IDictionary<string, string>? FieldLinksDictionary { get; set; }
    
        private IDictionary<string, string>? ArmLinksDictionary { get; set; }
        
        private FieldDto[]? _fields;

        private WorkStationDto[]? _workstations;
        
        // Значения в плитках
        int GeneralCount, AverageUrgent, AlarmsCountArm1, AlarmsCountArm2, AlarmsCountArm3, AlarmsCountArm4;

        [Parameter] public bool IsEnableRenderChart { get; set; } = false;
        public Dictionary<DateTimeOffset, IncomingAlarmDto[]>? IncomingAlarms { get; set; }

        protected override async Task OnInitializedAsync()
        {
            To = DateTimeOffset.Now.AddDays(-1);
            To = new DateTimeOffset(To.Year, To.Month, To.Day, 23, 59, 59,
                TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));
            From = DateTimeOffset.Now.AddDays(-1);
            From = new DateTimeOffset(From.Year, From.Month, From.Day, 0, 0, 0,
                TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));
            
            To = new DateTimeOffset(2023, 8, 7, 22, 0, 0, DateTimeOffset.Now.Offset);
            From = new DateTimeOffset(2023, 8, 1, 22, 0, 0, DateTimeOffset.Now.Offset);
        }

        protected override async Task OnParametersSetAsync()
        {
            IsEnableRenderChart = false;
            SpinnerService.Show();
            StateHasChanged();
            await SetFieldWithArm();
            GeneralCount = 0;
            AverageUrgent = await SetAverageUrgent();
            
            var incomingAlarmsResult = await IncomingAlarmService.GetCountInHour(new GetIncomingAlarmsByDatesQuery
            {
                WorkStationId = 1,
                ActivationFrom = From,
                ActivationTo = To,
            });
            
            if (incomingAlarmsResult.Success)
            {
                IncomingAlarms = incomingAlarmsResult.Payload;

                foreach (var alarmsOnHour in IncomingAlarms)
                {
                    if (alarmsOnHour.Value is { Length: > 0 })
                    {
                        GeneralCount += alarmsOnHour.Value.Length;
                        AlarmsCountArm1 += alarmsOnHour.Value.Length;
                    }
                }
            }

            SpinnerService.Hide();
            IsEnableRenderChart = true;
            StateHasChanged();
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

            ArmId ??= _workstations?.FirstOrDefault()?.Id;

            ArmName = _workstations?.FirstOrDefault(ws => ws.Id == ArmId)?.Name;
        
            FillLinks();
        }

        private void FillLinks()
        {
            if (_fields != null)
            {
                FieldLinksDictionary = _fields.ToDictionary(field => field.Name, field => $"/reports/?fieldId={field.Id}");
            }
        
            if (_workstations != null)
            {
                ArmLinksDictionary = _workstations.ToDictionary(workStation => workStation.Name, workStation => $"/reports/?fieldId={FieldId}&armId={workStation.Id}");
            }
        }
        
        private async Task SetDateTime(int days)
        {
            To = DateTimeOffset.Now.AddDays(-1);
            To = new DateTimeOffset(To.Year, To.Month, To.Day, 23, 59, 59,
                TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));
            From = DateTimeOffset.Now.AddDays(-days);
            From = new DateTimeOffset(From.Year, From.Month, From.Day, 0, 0, 0,
                TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));

            await OnParametersSetAsync();
        }

        async Task<int> SetAverageUrgent()
        {
            var incomingAlarmsResult = await IncomingAlarmService.GetCountInHour(new GetIncomingAlarmsByDatesQuery
            {
                WorkStationId = 1,
                ActivationFrom = From,
                ActivationTo = To,
            });
            
            if (incomingAlarmsResult.Success)
            {
                var incomingAlarms = incomingAlarmsResult.Payload;
                
                int countUrgent = 0;
                
                foreach (var alarmsOnHour in incomingAlarms)
                {
                    if (alarmsOnHour.Value is { Length: > 0 })
                        foreach (var alarm in alarmsOnHour.Value)
                        {
                            if (alarm.Priority == PriorityType.Urgent)
                            {
                                countUrgent += 1;
                            }
                        }
                }

                if (incomingAlarms.Count > 0)
                    return countUrgent / incomingAlarms.Count;
            }
            
            return -1;
        }
        
        private async Task DownloadFileFromStream()
        {
            
        }
    }
}