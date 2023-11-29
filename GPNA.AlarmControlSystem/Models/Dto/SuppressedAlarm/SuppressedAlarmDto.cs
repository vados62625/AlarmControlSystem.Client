using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm;

/// <summary>
/// Подавленные сигнализации
/// </summary>
public class SuppressedAlarmDto : AlarmBaseDto
{
    /// <summary>
    /// Время события активации сигнализации
    /// </summary>
    public DateTimeOffset DateTimeStart { get; set; }

    /// <summary>
    /// Время события снятия с подавления сигнализации
    /// </summary>
    public DateTimeOffset DateTimeEnd { get; set; }

    /// <summary>
    /// Длительность
    /// </summary>
    public TimeSpan? Duration => DateTimeOffset.Now - DateTimeStart;

    /// <summary>
    /// Комментарии
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Статус сигнализации
    /// </summary>
    public StatusAlarmType StatusAlarm { get; set; }
}