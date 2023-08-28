using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface IActiveAlarmService : ICrudBase<ActiveAlarmDto>
    {

        Task<Result<CountAlarmsOnDate[]>> GetCountActiveAlarmsByDates(GetCountActiveAlarmsByDatesQuery content);
    }
}
