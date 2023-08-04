using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto
{
    public class BufferAlarmDto : AlarmBaseDto
    {
        /// <summary>
        /// Id рабочей станции
        /// </summary>
        public int WorkStationId { get; set; }
        
        /// <summary>
        /// Время события активации сигнализации
        /// </summary>
        public DateTimeOffset DateTimeActivation { get; set; }

        /// <summary>
        /// Время события подавления сигнализации
        /// </summary>
        public DateTimeOffset? DateTimeSuppression { get; set; }

        /// <summary>
        /// Время события нормализации сигнализации
        /// </summary>
        public DateTimeOffset? DateTimeNormalization { get; set; }

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
