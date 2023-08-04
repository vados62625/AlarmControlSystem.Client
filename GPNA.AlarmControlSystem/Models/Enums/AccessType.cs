using System.ComponentModel;

namespace GPNA.AlarmControlSystem.Models.Enums
{
    public enum AccessType
    {
        [Description("Отсутствует")]
        None = 0,
        [Description("Оператор")]
        Operator = 1,
        [Description("Админ")]
        Admin = 2,
        [Description("Супер Админ")]
        SuperAdmin = 3,
    }
}
