namespace GPNA.AlarmControlSystem.Models.Dto.KpiSettings;

/// <summary>
/// Настройки таблицы переменных
/// </summary>
public class TagTableSettingsDto : EntityDtoBase
{
    /// <summary>
    /// Id рабочей станции
    /// </summary>
    public int WorkStationId { get; set; }
    
    /// <summary>
    /// Заполненность оценки рисков
    /// </summary>
    public int RiskAssessment { get; set; }
}