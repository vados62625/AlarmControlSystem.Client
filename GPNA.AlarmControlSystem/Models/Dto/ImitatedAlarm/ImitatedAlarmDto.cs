using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;

namespace GPNA.AlarmControlSystem.Models.Dto.ImitatedAlarm;

/// <summary>
/// Активные сигнализации
/// </summary>
public class ImitatedAlarmDto : BufferAlarmDto
{
    /// <summary>
    /// Длительность
    /// </summary>
    public TimeSpan? Duration => DateTimeOffset.Now - DateTimeStart;
}