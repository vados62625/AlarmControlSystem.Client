using System.ComponentModel;

namespace LocalDbStorage.Repositories.Models.Enum;

public enum PriorityType
{
    [Description("Отсутствует")]
    None,
    [Description("Информационный")]
    Information,
    [Description("Низкий")]
    Low,
    [Description("Высокий")]
    High,
    [Description("Критический")]
    Urgent
}