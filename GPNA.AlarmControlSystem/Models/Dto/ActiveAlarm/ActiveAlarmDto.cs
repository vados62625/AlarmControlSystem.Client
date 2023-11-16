using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;

namespace GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;

/// <summary>
/// Активные сигнализации
/// </summary>
public class ActiveAlarmDto : BufferAlarmDto
{
    /// <summary>
    /// Длительность
    /// </summary>
    public TimeSpan? Duration => DateTime.Now - DateTimeActivation.DateTime;
}