using System.ComponentModel;

namespace LocalDbStorage.Repositories.Models.Enum
{
    public enum StatusAlarm
    {
        [Description("Не определено")]
        Undefined,
        [Description("В работе")]
        InWork,
        [Description("Сделана")]
        Done
    }
}
