namespace GPNA.AlarmControlSystem.Models.Dto.KpiSettings;

/// <summary>
/// Настройки журнала сигнализаций
/// </summary>
public class AlarmJournalSettingsDto : EntityDtoBase
{
    /// <summary>
    /// Id рабочей станции
    /// </summary>
    public int WorkStationId { get; set; }
    
    /// <summary>
    /// Критичный приоритет
    /// </summary>
    public int UrgentPriority { get; set; }
    
    /// <summary>
    /// Высокий приоритет
    /// </summary>
    public int HighPriority { get; set; }
    
    /// <summary>
    /// Низкий приоритет
    /// </summary>
    public int LowPriority { get; set; }
}