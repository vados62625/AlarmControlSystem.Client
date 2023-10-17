using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm
{
    public class GetSuppressedAlarmsListQuery : EntityQueryBase
    {
        /// <summary>
        /// Имя тега
        /// </summary>
        public string? TagName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public StateType? State { get; set; }

        /// <summary>
        /// Приоритет
        /// </summary>
        public PriorityType? Priority { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Статус сигнализации
        /// </summary>
        public StatusAlarmType? StatusAlarm { get; set; }
    }
}
