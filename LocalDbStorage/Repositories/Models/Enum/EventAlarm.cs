using System.ComponentModel;

namespace LocalDbStorage.Repositories.Models.Enum
{
    public enum EventAlarm
    {
        [Description("Активация")]
        Activation,
        [Description("Подавление")]
        Suppression,
        [Description("Нормализация")]
        Normalization
    }
}
