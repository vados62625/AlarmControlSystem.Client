namespace GPNA.AlarmControlSystem.Models.Dto.KpiSettings;

/// <summary>
/// Настройки мониторинга
/// </summary>
public class MonitoringSettingsDto : EntityDtoBase
{
    /// <summary>
    /// Id рабочей станции
    /// </summary>
    public int WorkStationId { get; set; }
    
    /// <summary>
    /// Входящие сигнализации. Количество повторений
    /// </summary>
    public int RepeatCount { get; set; }
    
    /// <summary>
    /// Активные сигнализации. Длительность, ч
    /// </summary>
    public int ActiveAlarms { get; set; }
    
    /// <summary>
    /// Имитационные параметры. Длительность, ч
    /// </summary>
    public int ImitationParams { get; set; }
    
    /// <summary>
    /// Подавленные сигнализации. Длительность, ч
    /// </summary>
    public int SuppressedAlarms { get; set; }
}