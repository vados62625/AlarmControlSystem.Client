namespace GPNA.AlarmControlSystem.Models.Dto.Queries;

public class GetAlarmsCountForFieldQuery
{
    public int FieldId { get; set; }
    public DateTimeOffset DateTimeStart { get; set; }
    public DateTimeOffset DateTimeEnd { get; set; }
}