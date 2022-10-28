using System.ComponentModel;

namespace LocalDbStorage.Repositories.Models.Enum;

public enum Validate
{
    [Description("Не проверенно")]
    NoneCheck,
    [Description("Валидный")]
    Valid,
    [Description("Не валидный")]
    NotValid,
    [Description("Новая запись")]
    NewTag
}
