using GPNA.AlarmControlSystem.Models.Dto.Tag;

namespace GPNA.AlarmControlSystem.Application.Dto.Tag;

public class TagsCollection
{
    public IEnumerable<TagDto> Items { get; set; }
    
    /// <summary>
    /// Kpi
    /// </summary>
    public double Kpi { get; set; }
    
    /// <summary>
    /// Нет оценки последствий
    /// </summary>
    public int InvalidConsequence { get; set; }
    
    /// <summary>
    /// Некорректное описание
    /// </summary>
    public int InvalidDesc { get; set; }
    
    /// <summary>
    /// Некорректный приоритет
    /// </summary>
    public int InvalidPriority { get; set; }
    
    /// <summary>
    /// Некорректная уставка
    /// </summary>
    public int InvalidAlarmLimit { get; set; }
    
    /// <summary>
    /// Непроверенные
    /// </summary>
    public int Unchecked { get; set; }
    
    /// <summary>
    /// Новые
    /// </summary>
    public int New { get; set; }
    
    /// <summary>
    /// Количество страниц
    /// </summary>
    public int PagesCount { get; set; } = 1;
}