using System.ComponentModel;

namespace GPNA.AlarmControlSystem.Models.Enums
{
    public enum StatusAlarmType
    {
        [Description("Не определено")]
        Undefined = 0,
        [Description("В работе")]
        InWork = 1,
        [Description("Сделана")]
        Done = 2
    }
}
