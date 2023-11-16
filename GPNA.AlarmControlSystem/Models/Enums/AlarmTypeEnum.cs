using System.ComponentModel;

namespace GPNA.AlarmControlSystem.Models.Enums
{
    public enum AlarmTypeEnum
    {
        [Description("Входящие")]
        Incoming = 0,
        [Description("Активные")]
        Active = 1,
        [Description("Имитация параметров")]
        Simulation = 2,
        [Description("Подавленные")]
        Suppressed = 3,
    }
}
