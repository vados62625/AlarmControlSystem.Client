using GPNA.AlarmControlSystem.Application.Dto;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface IIncomingAlarmService : ICrudBase<IncomingAlarmDto>
    {
        Task<Result<CountAlarmsOnDate[]>> GetCountIncomingAlarmsByDates(GetAlarmsCollectionQuery content);
        
        Task<Result<CountAlarmsOnPriority[]>> GetCountIncomingAlarmsByPriorities(GetAlarmsCollectionQuery content);

        Task<Result<AlarmsCollection<IncomingAlarmDto>>> GetAlarmsPerDate(GetAlarmsCollectionQuery content);

        Task<Result<Dictionary<DateTimeOffset, IncomingAlarmDto[]>>> GetCountInHour(
            GetAlarmsCollectionQuery content);
    }
}
