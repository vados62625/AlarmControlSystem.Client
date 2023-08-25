using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;

namespace GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;

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