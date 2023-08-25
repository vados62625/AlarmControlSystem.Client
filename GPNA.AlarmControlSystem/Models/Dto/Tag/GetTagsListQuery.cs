using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.Tag
{
    public class GetTagsListQuery
    {
        /// <summary>
        /// Id рабочей станции
        /// </summary>
        public int WorkStationId { get; set; }

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
        public int? Page { get; set; }
        public int? ItemsOnPage { get; set; }
        public string? Suggest { get; set; }
    }
}
