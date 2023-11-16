using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface IActiveAlarmService : ICrudBase<ActiveAlarmDto>
    {

        Task<Result<CountAlarmsOnDate[]>> GetCountActiveAlarmsByDates(GetCountActiveAlarmsByDatesQuery content);
        Task<Result<AlarmsCollection<ActiveAlarmDto>>> GetActiveAlarmsPerDate(GetIncomingAlarmsByDatesQuery content);
    }
}
