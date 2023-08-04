namespace GPNA.AlarmControlSystem.Models.Dto;

/// <summary>
/// Входящие аварии
/// </summary>
public class IncomingAlarmDto : BufferAlarmDto
{
    /// <summary>
    /// Дата
    /// </summary>
    public DateTime EndDate
    {
        get => DateTimeNormalization.Value.DateTime;

        set { }
    }
}