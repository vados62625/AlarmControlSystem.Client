using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface ISuppressedAlarmService : ICrudBase<SuppressedAlarmDto>
    {
        Task<Result<CountAlarmsOnDate[]>> GetCountSuppressedAlarmsByDates(GetCountSuppressedAlarmsByDatesQuery content);
    }
}
