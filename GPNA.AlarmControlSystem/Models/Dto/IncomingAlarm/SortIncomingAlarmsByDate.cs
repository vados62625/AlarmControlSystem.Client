namespace GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm
{
    public class SortIncomingAlarmsByDate : CountAlarmsOnDate
    {
        private IEnumerable<IncomingAlarmDto> IncomingAlarms { get; set; }
    }
}
    