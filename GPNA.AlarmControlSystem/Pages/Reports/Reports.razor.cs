using GPNA.AlarmControlSystem.Services;
using LocalDbStorage.Dto;
using LocalDbStorage.Interfaces;
using Microsoft.AspNetCore.Components;

namespace GPNA.AlarmControlSystem.Pages.Reports
{
    public partial class Reports : ComponentBase
    {
        [Inject] IIncomingAlarmService IncomingAlarmService { get; set; } = null!;
        [Inject] protected ISpinnerService SpinnerService { get; set; } = default!;

        [Parameter] public DateTime From { get; set; }
        [Parameter] public DateTime To { get; set; }
        static int inputKpi = 12;

        // Значения в плитках
        int GeneralCount, AverageUrgent, AlarmsCountArm1, AlarmsCountArm2, AlarmsCountArm3, AlarmsCountArm4;

        [Parameter] public bool IsEnableRenderChart { get; set; } = false;
        public Dictionary<DateTime, List<IncomingAlarmDto>>? IncomingAlarms { get; set; }

        protected override async Task OnInitializedAsync()
        {
            To = DateTime.Now.AddDays(-1);
            To = new DateTime(To.Year, To.Month, To.Day, 23, 59, 59);
            From = DateTime.Now.AddDays(-1);
            From = new DateTime(From.Year, From.Month, From.Day, 0, 0, 0);            
        }

        protected override async Task OnParametersSetAsync()
        {
            IsEnableRenderChart = false;
            SpinnerService.Show();
            StateHasChanged();
            GeneralCount = 0;
            AverageUrgent = SetAverageUrgent().Result;
            IncomingAlarms = await IncomingAlarmService.GetCountInHour(1, From, To);

            foreach (var alarmsOnHour in IncomingAlarms)
            {
                if (alarmsOnHour.Value != null && alarmsOnHour.Value.Count > 0)
                { 
                    GeneralCount += alarmsOnHour.Value.Count;
                    AlarmsCountArm1 += alarmsOnHour.Value.Count;                        
                }
            }

            SpinnerService.Hide();
            IsEnableRenderChart = true;
            StateHasChanged();
        }

        async void SetDateTime(int days)
        {
            To = DateTime.Now.AddDays(-1);
            To = new DateTime(To.Year, To.Month, To.Day, 23, 59, 59);
            From = DateTime.Now.AddDays(-days);
            From = new DateTime(From.Year, From.Month, From.Day, 0, 0, 0);

            await OnParametersSetAsync();
        }

        async Task<int> SetAverageUrgent()
        {
            var incomingAlarms = await IncomingAlarmService.GetCountInHour(1, DateTime.Now.AddHours(-720), DateTime.Now);
            int countUrgent = 0;
            foreach (var alarmsOnHour in incomingAlarms)
            {
                if (alarmsOnHour.Value != null && alarmsOnHour.Value.Count > 0)
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
            return -1;
        }
    }
}
