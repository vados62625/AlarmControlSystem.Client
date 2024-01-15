using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm;

/// <summary>
/// Подавленные сигнализации
/// </summary>
public class SuppressedAlarmDto : BufferAlarmDto
{
    /// <summary>
    /// Длительность
    /// </summary>
    public TimeSpan? Duration => DateTimeOffset.Now - DateTimeStart;
}