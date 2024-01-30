using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.Tag;

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
    }
}
