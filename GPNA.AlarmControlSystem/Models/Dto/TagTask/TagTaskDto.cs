using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.Email;
using GPNA.AlarmControlSystem.Models.Dto.Tag;
using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.TagTask
{
    /// <summary>
    /// Теги
    /// </summary>
    public class TagTaskDto : EntityDtoBase
    {
        /// <summary>
        /// Тег
        /// </summary>
        public TagDto? Tag { get; set; }
        
        /// <summary>
        /// Связанная сигнализация
        /// </summary>
        public virtual BufferAlarmDto? BufferAlarm { get; set; }
        
        /// <summary>
        /// Номер
        /// </summary>
        public int Number { get; set; }
    
        /// <summary>
        /// В архиве
        /// </summary>
        public bool Archived { get; set; }
        
        /// <summary>
        /// Событие последней сигнализации тега на момент создания задачи
        /// </summary>
        public AlarmType EventAlarm { get; set; }

        /// <summary>
        /// Связанные пользователи
        /// </summary>
        public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();

        /// <summary>
        /// Связанные сообщения
        /// </summary>
        public virtual ICollection<EmailMessageDto> EmailMessages { get; set; } = new List<EmailMessageDto>();
    }
}
