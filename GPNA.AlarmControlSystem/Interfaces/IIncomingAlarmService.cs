using GPNA.AlarmControlSystem.Application.Dto;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface IIncomingAlarmService : ICrudBase<IncomingAlarmDto>
    {
        Task<Result<CountAlarmsOnDate[]>> GetCountIncomingAlarmsByDates(GetIncomingAlarmsByDatesQuery content);
        
        Task<Result<CountAlarmsOnPriority[]>> GetCountIncomingAlarmsByPriorities(GetIncomingAlarmsByDatesQuery content);

        Task<Result<IncomingAlarmDto[][]>> GetAlarmsPerDate(GetIncomingAlarmsByDatesQuery content);

        Task<Result<Dictionary<DateTimeOffset, IncomingAlarmDto[]>>> GetCountInHour(
            GetIncomingAlarmsByDatesQuery content);
    }
}
