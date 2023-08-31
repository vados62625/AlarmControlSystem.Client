using GPNA.AlarmControlSystem.Application.Dto;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface IIncomingAlarmService : ICrudBase<IncomingAlarmDto>
    {
        Task<Result<CountAlarmsOnDate[]>> GetCountIncomingAlarmsByDates(GetCountIncomingAlarmsByDatesQuery content);
        Task<Result<CountAlarmsOnPriority[]>> GetCountIncomingAlarmsByPriorities(GetCountIncomingAlarmsByDatesQuery content);

        Task<List<List<IncomingAlarmDto>>> GetAlarmsPerDate(int idWorkStation, DateTime from, DateTime to);

        Task<Dictionary<DateTime, List<IncomingAlarmDto>>>
            GetCountInHour(int idWorkStation, DateTime from, DateTime to);
    }
}
