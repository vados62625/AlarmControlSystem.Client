using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.BufferAlarms
{
    public class GetAlarmsCollectionQueryBase : EntityQueryBase
    {
        /// <summary>
        /// Статус сигнализации
        /// </summary>
        public StatusAlarmType? StatusAlarm { get; set; }
        
        /// <summary>
        /// Приоритет
        /// </summary>
        public List<PriorityType>? Priority { get; set; }
        
        /// <summary>
        /// Состояние
        /// </summary>
        public List<StateType>? State { get; set; }
    
        /// <summary>
        /// Имя поля для сортировки
        /// </summary>
        public string? OrderPropertyName { get; set; }
    
        /// <summary>
        /// Сортировать по убыванию
        /// </summary>
        public bool OrderByDescending { get; set; }
        
        /// <summary>
        /// Выводить передачи смены
        /// </summary>
        public bool DisplayShifts { get; set; }
    }
}
