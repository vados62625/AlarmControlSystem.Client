namespace GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm
{
    public class GetCountSuppressedAlarmsByDatesQuery
    {
        public int WorkStationId { get; set; }
        public virtual DateTimeOffset DateTimeStart { get; set; }
        public virtual DateTimeOffset DateTimeEnd { get; set; }
    }
}
