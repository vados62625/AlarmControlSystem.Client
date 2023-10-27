using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;

namespace GPNA.AlarmControlSystem.Pages.Reports
{
    public partial class Reports : ComponentBase
    {
        [Inject] IBufferAlarmService AlarmService { get; set; } = null!;
        [Inject] IIncomingAlarmService IncomingAlarmService { get; set; } = null!;
        [Inject] protected ISpinnerService SpinnerService { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public int? FieldId { get; set; }
        [Parameter] public DateTimeOffset From { get; set; }
        [Parameter] public DateTimeOffset To { get; set; }
        static int inputKpi = 12;

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
        }

        protected override async Task OnParametersSetAsync()
        {
            IsEnableRenderChart = false;
            SpinnerService.Show();
            StateHasChanged();
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
                    if (alarmsOnHour.Value != null && alarmsOnHour.Value.Length > 0)
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
                    if (alarmsOnHour.Value != null && alarmsOnHour.Value.Length > 0)
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