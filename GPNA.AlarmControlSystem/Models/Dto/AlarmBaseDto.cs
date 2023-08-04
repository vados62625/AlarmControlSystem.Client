using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto
{
    public class AlarmBaseDto : EntityDtoBase
    {
        /// <summary>
        /// Имя тега
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public StateType State { get; set; }

        /// <summary>
        /// Приоритет
        /// </summary>
        public PriorityType Priority { get; set; }

        /// <summary>
        /// Предел
        /// </summary>
        public string? AlarmLimit { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string? Value { get; set; }
    }
}
