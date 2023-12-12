using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.BufferAlarms
{
    public class BufferAlarmDto : AlarmBaseDto
    {
        
        /// <summary>
        /// Id тега
        /// </summary>
        public int TagId { get; set; }
        
        /// <summary>
        /// Id рабочей станции
        /// </summary>
        public int WorkStationId { get; set; }
        
        /// <summary>
        /// Время события активации сигнализации
        /// </summary>
        public DateTimeOffset DateTimeStart { get; set; }

        /// <summary>
        /// Время события подавления сигнализации
        /// </summary>
        public DateTimeOffset? DateTimeSuppression { get; set; }

        /// <summary>
        /// Время события нормализации сигнализации
        /// </summary>
        public DateTimeOffset? DateTimeEnd { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Статус сигнализации
        /// </summary>
        public StatusAlarmType StatusAlarm { get; set; }

        /// <summary>
        /// Идентификатор в источнике
        /// </summary>
        public string IdInSourse { get; set; }
    }
}
