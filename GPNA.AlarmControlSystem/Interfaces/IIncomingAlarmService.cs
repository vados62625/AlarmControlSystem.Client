using GPNA.AlarmControlSystem.Application.Dto;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface IIncomingAlarmService : ICrudBase<IncomingAlarmDto>
    {
        Task<Result<CountAlarmsOnDate[]>> GetCountIncomingAlarmsByDates(GetAlarmsCollectionQueryBase content);
        
        Task<Result<CountAlarmsOnPriority[]>> GetCountIncomingAlarmsByPriorities(GetAlarmsCollectionQueryBase content);

        Task<Result<AlarmsCollection<IncomingAlarmDto[]>>> GetIncomingAlarmsCollection(GetAlarmsCollectionQueryBase content);

        Task<Result<Dictionary<DateTimeOffset, IncomingAlarmDto[]>>> GetCountInHour(
            GetAlarmsCollectionQueryBase content);
        
        Task<Result<Dictionary<int, Dictionary<AlarmType, Dictionary<DateTime, int>>>>> GetExpiredCountInHour(
            GetExpiredAlarmsByDatesQuery content);
        
        Task<Result<Dictionary<int, Dictionary<AlarmType, Dictionary<DateTime, int>>>>> GetExpiredCountPerWeek(
            GetExpiredAlarmsByDatesQuery content);
    }
}
