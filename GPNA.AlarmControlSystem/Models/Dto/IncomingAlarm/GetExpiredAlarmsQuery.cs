using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm
{
    public class GetExpiredAlarmsByDatesQuery
    {
        /// <summary>
        /// Id месторождения
        /// </summary>
        public int FieldId { get; set; }
        
        /// <summary>
        /// Начало периода
        /// </summary>
        public DateTimeOffset DateTimeStart { get; set; }
        
        /// <summary>
        /// Конец периода
        /// </summary>
        public DateTimeOffset DateTimeEnd { get; set; }
    }
}
