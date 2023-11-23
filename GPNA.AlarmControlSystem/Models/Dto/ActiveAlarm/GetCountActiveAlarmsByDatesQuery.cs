namespace GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm
{
    public class GetCountActiveAlarmsByDatesQuery
    {
        public int WorkStationId { get; set; }
        public virtual DateTimeOffset DateTimeStart { get; set; }
        public virtual DateTimeOffset DateTimeEnd { get; set; }
    }
}
