namespace GPNA.AlarmControlSystem.Models.Dto.Queries;

public class GetAlarmsCountForFieldQuery
{
    public int FieldId { get; set; }
    public DateTimeOffset ActivationFrom { get; set; }
    public DateTimeOffset ActivationTo { get; set; }
}