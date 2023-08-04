using System.ComponentModel;

namespace GPNA.AlarmControlSystem.Models.Enums
{
    public enum ValidateType
    {
        [Description("Не проверенно")]
        NoneCheck = 0,
        [Description("Валидный")]
        Valid = 1,
        [Description("Не валидный")]
        NotValid = 2,
        [Description("Новая запись")]
        NewTag = 3
    }
}
