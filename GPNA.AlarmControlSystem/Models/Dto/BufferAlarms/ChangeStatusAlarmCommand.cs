using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.BufferAlarms
{
    public class ChangeStatusAlarmCommand
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Статус сигнализации
        /// </summary>
        public StatusAlarmType StatusAlarm { get; set; }
    }
}
