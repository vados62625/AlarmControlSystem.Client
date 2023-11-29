using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.ImitatedAlarm;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface IImitatedAlarmService : ICrudBase<ImitatedAlarmDto>
    {

        Task<Result<CountAlarmsOnDate[]>> GetCountImitatedAlarmsByDates(GetCountImitatedAlarmsByDatesQuery content);
        Task<Result<AlarmsCollection<ImitatedAlarmDto>>> GetImitatedAlarmsPerDate(GetIncomingAlarmsByDatesQuery content);
    }
}
