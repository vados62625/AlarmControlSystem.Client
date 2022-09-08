using LocalDbStorage.Repositories.Base;
using LocalDbStorage.Repositories.Models.Enum;

namespace LocalDbStorage.Repositories.Models;

/// <summary>
/// Теги
/// </summary>
public partial class Tag : EntityBase
{
    /// <summary>
    /// Позиция
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// Имя тега
    /// </summary>
    public string TagName { get; set; }

    /// <summary>
    /// Описание 
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// ед. измерения 
    /// </summary>
    public string? Unit { get; set; }

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
    /// Шкала
    /// </summary>
    public string? Scale { get; set; }

    /// <summary>
    /// Последствие
    /// </summary>
    public string? Consequence { get; set; }

    /// <summary>
    /// Действие полевого оператора
    /// </summary>
    public string? ActionFieldOperator { get; set; }

    /// <summary>
    /// Действие оператора АРМ
    /// </summary>
    public string? ActionArmOperator { get; set; }

    /// <summary>
    /// Информация
    /// </summary>
    public string? Inform { get; set; }

    /// <summary>
    /// Время на реакцию
    /// </summary>
    public string? ReactionTime { get; set; }

    /// <summary>
    /// Состояние проверки на валидность информации
    /// </summary>
    public Validate Validate { get; set; }
}