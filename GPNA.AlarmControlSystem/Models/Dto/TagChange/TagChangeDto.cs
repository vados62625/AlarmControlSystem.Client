using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.Tag;
using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Application.Dto.TagChange
{
    /// <summary>
    /// Задача
    /// </summary>
    public class TagChangeDto : EntityDtoBase
    {
        /// <summary>
        /// Id тега
        /// </summary>
        public int TagId { get; set; }
        
        public TagDto? Tag { get; set; }

        /// <summary>
        /// Id инициатора
        /// </summary>
        public int InitiatorId { get; set; }

        public virtual UserDto? Initiator { get; set; }

        /// <summary>
        /// Id исполнителя
        /// </summary>
        public int ExecutorId { get; set; }

        public virtual UserDto? Executor { get; set; }

        /// <summary>
        /// В архиве
        /// </summary>
        public bool Archived { get; set; }
        
        /// <summary>
        /// Новое описание
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Новый приоритет
        /// </summary>
        public PriorityType? Priority { get; set; }
        
        /// <summary>
        /// Новая уставка
        /// </summary>
        public string? AlarmLimit { get; set; }
    }
}
