namespace GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;

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