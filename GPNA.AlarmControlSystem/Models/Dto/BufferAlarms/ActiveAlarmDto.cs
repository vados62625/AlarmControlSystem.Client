using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;

/// <summary>
/// Активные сигнализации
/// </summary>
public class ActiveAlarmDto : AlarmBaseDto
{
    /// <summary>
    /// Время события активации сигнализации
    /// </summary>
    public DateTimeOffset DateTimeActivation { get; set; }

    /// <summary>
    /// Длительность
    /// </summary>
    public TimeSpan? Duration
    {
        get => DateTime.Now - DateTimeActivation.DateTime;
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