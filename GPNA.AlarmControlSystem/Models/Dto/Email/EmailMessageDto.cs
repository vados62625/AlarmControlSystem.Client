namespace GPNA.AlarmControlSystem.Models.Dto.Email;

public class EmailMessageDto : EntityDtoBase
{
    public DateTimeOffset DateTimeSent { get; set; }
    public string Consumer { get; set; }
    public string Producer { get; set; }
    public string Message { get; set; }
    public int BufferAlarmId { get; set; }
}