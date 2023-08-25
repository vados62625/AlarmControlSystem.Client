namespace GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm
{
    public class GetCountActiveAlarmsByDatesQuery
    {
        public int WorkStationId { get; set; }
        public virtual DateTimeOffset ActivationFrom { get; set; }
        public virtual DateTimeOffset ActivationTo { get; set; }
    }
}
