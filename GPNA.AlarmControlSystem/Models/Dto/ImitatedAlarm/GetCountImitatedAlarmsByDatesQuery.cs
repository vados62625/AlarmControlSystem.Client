namespace GPNA.AlarmControlSystem.Models.Dto.ImitatedAlarm
{
    public class GetCountImitatedAlarmsByDatesQuery
    {
        public int WorkStationId { get; set; }
        public virtual DateTimeOffset DateTimeStart { get; set; }
        public virtual DateTimeOffset DateTimeEnd { get; set; }
    }
}
