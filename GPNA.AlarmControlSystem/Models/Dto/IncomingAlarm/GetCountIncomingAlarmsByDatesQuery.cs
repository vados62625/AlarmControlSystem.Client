namespace GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm
{
    public class GetCountIncomingAlarmsByDatesQuery
    {
        public int WorkStationId { get; set; }
        public virtual DateTimeOffset ActivationFrom { get; set; }
        public virtual DateTimeOffset ActivationTo { get; set; }
    }
}
