namespace GPNA.AlarmControlSystem.Models.Dto.KpiSettings;

/// <summary>
/// Настройки задач
/// </summary>
public class TaskSettingsDto : EntityDtoBase
{
    /// <summary>
    /// Id рабочей станции
    /// </summary>
    public int WorkStationId { get; set; }
        
    /// <summary>
    /// Время ответа на задачу, сутки
    /// </summary>
    public int ResponseDays { get; set; }
    
    /// <summary>
    /// Просроченные
    /// </summary>
    public int Overdue { get; set; }
    
    /// <summary>
    /// Выполненные вовремя
    /// </summary>
    public int CompletedOnTime { get; set; }
}