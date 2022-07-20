

using GPNA.ACSAPI.Repositories.Base;
using LocalDbStorage.Repositories.Models.Enum;

namespace GPNA.ACSAPI.Repositories.Models;

/// <summary>
/// Входящие аварии
/// </summary>
public class IncomingAlarm : EntityBase
{
    /// <summary>
    /// Id рабочей станции
    /// </summary>
    public int IdWorkStation { get; set; }

    /// <summary>
    /// Дата
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Имя тега
    /// </summary>
    public string TagName { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Состояние
    /// </summary>
    public StateType State { get; set; }

    /// <summary>
    /// Приоритет
    /// </summary>
    public PriorityType Priority { get; set; }

    /// <summary>
    /// Предел
    /// </summary>
    public string? AlarmLimit { get; set; }

    /// <summary>
    /// Значение
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Комментарии
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Тест
    /// </summary>
    public bool Test { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public int? Count { get; set; }

    public virtual WorkStation? WorkStation { get; set; }

}