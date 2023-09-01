namespace GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm
{
    public class GetCountIncomingAlarmsByDatesQuery
    {
        public int WorkStationId { get; set; }
        
        public DateTimeOffset ActivationFrom { get; set; }
        
        public DateTimeOffset ActivationTo { get; set; }
    }
}
