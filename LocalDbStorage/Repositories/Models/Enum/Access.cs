using System.ComponentModel;

namespace LocalDbStorage.Repositories.Models.Enum;

public enum Access
{
    [Description("Отсутствует")]
    None,
    [Description("Оператор")]
    Operator,
    [Description("Админ")]
    Admin,
    [Description("Супер Админ")]
    SuperAdmin
}