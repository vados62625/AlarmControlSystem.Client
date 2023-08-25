namespace GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm
{
    public class GetCountSuppressedAlarmsByDatesQuery
    {
        public int WorkStationId { get; set; }
        public virtual DateTimeOffset ActivationFrom { get; set; }
        public virtual DateTimeOffset ActivationTo { get; set; }
    }
}
