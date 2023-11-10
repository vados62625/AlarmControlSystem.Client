namespace GPNA.AlarmControlSystem.Models.Dto.KpiSettings;

/// <summary>
/// Настройки отчетов
/// </summary>
public class ReportSettingsDto : EntityDtoBase
{
    /// <summary>
    /// Id рабочей станции
    /// </summary>
    public int WorkStationId { get; set; }
    
    /// <summary>
    /// В час
    /// </summary>
    public int InHour { get; set; }

    /// <summary>
    /// Сутки
    /// </summary>
    public int Days { get; set; }

    /// <summary>
    /// Просроченные активные
    /// </summary>
    public int ActiveOverdue { get; set; }

    /// <summary>
    /// Просроченные имитационные
    /// </summary>
    public int ImitationOverdue { get; set; }

    /// <summary>
    /// Просроченные подавленные
    /// </summary>
    public int SuppressedOverdue { get; set; }
}