namespace GPNA.AlarmControlSystem.Models.Dto
{
    public class CountAlarmsOnDate
    {
        public DateTimeOffset DateTime { get; set; }
        public int Count { get; set; }
    }
}
