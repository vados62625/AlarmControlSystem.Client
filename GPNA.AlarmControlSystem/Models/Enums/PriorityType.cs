using System.ComponentModel;

namespace GPNA.AlarmControlSystem.Models.Enums
{
    public enum PriorityType
    {
        [Description("Отсутствует")]
        None = 0,
        [Description("Инфо")]
        Information = 1,
        [Description("Низкий")]
        Low = 2,
        [Description("Высокий")]
        High = 3,
        [Description("Критичный")]
        Urgent = 4
    }
}
