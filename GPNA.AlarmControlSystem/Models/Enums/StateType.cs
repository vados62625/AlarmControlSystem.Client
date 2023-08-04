using System.ComponentModel;

namespace GPNA.AlarmControlSystem.Models.Enums
{
    public enum StateType
    {
        [Description("Отсутствует")]
        none = 0,
        [Description("L")]
        L = 1,
        [Description("LL")]
        LL = 2,
        [Description("H")]
        H = 3,
        [Description("HH")]
        HH = 4,
        [Description("RSLO")]
        RSLO = 5,
        [Description("RSHI")]
        RSHI = 6,
        [Description("Discr")]
        Discr = 7,
        [Description("Comms")]
        Comms = 8
    }
}
