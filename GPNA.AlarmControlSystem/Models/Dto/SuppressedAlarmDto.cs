using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto;

/// <summary>
/// Подавленные сигнализации
/// </summary>
public class SuppressedAlarmDto : AlarmBaseDto
{
    /// <summary>
    /// Время события активации сигнализации
    /// </summary>
    public DateTimeOffset DateTimeActivation { get; set; }

    /// <summary>
    /// Время события подавления сигнализации
    /// </summary>
    public DateTimeOffset DateTimeSuppression { get; set; }

    /// <summary>
    /// Длительность
    /// </summary>
    public TimeSpan? Duration
    {
        get => DateTime.Now - DateTimeSuppression.DateTime;
        set
        {

        }
    }

    /// <summary>
    /// Комментарии
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Статус сигнализации
    /// </summary>
    public StatusAlarmType StatusAlarm { get; set; }
}