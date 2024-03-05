using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.TagTask
{
    public class GetTagTasksListQuery : EntityQueryBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int? Id { get; set; }
        
        /// <summary>
        /// В архиве
        /// </summary>
        public bool Archived { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public List<StateType>? State { get; set; }

        /// <summary>
        /// Тег
        /// </summary>
        public string? TagName { get; set; }

        /// <summary>
        /// Приоритет
        /// </summary>
        public List<PriorityType>? Priority { get; set; }
        
        /// <summary>
        /// Имя поля для сортировки
        /// </summary>
        public string? OrderPropertyName { get; set; }
    
        /// <summary>
        /// Сортировать по убыванию
        /// </summary>
        public bool OrderByDescending { get; set; }
    }
}
