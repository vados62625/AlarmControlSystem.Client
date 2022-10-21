using GPNA.AlarmControlSystem.Pages.Reports.Charts;
using GPNA.AlarmControlSystem.Services;
using LocalDbStorage.Dto;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using LocalDbStorage.Repositories.Models.Enum;
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
            From = DateTime.Now.AddDays(-7);
            From = new DateTime(From.Year, From.Month, From.Day, 0, 0, 0);
        }

        protected override async Task OnParametersSetAsync()
        {
            IsEnableRenderChart = false;
            SpinnerService.Show();
            GeneralCount = 0;
            AverageUrgent = 0;
            IncomingAlarms = await IncomingAlarmService.GetCountInHour(1, From, To);                       

            foreach (var alarmsOnHour in IncomingAlarms)
            {
                if (alarmsOnHour.Value!=null && alarmsOnHour.Value.Count > 0)
                    foreach (var alarm in alarmsOnHour.Value)
                    {
                        GeneralCount += 1;
                        AlarmsCountArm1 += 1;
                        if (alarm.Priority == PriorityType.Urgent)
                        {
                            AverageUrgent += 1;
                        }
                    }                
            }

            AverageUrgent = AverageUrgent / IncomingAlarms.Count;

            SpinnerService.Hide();
            IsEnableRenderChart = true;
        }
    }
}
