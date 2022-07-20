using System.ComponentModel;

namespace LocalDbStorage.Repositories.Models.Enum;

public enum StateType
{
    [Description("Отсутствует")]
    none,
    [Description("L")]
    L,
    [Description("LL")]
    LL,
    [Description("H")]
    H,
    [Description("HH")]
    HH
}