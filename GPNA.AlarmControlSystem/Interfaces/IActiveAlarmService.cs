using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface IActiveAlarmService
    {
        Task<Result<PageableCollectionDto<ActiveAlarmDto>>> GetActiveAlarmsByDates(
            GetActiveAlarmsListQuery content);

        Task<Result<CountAlarmsOnDate[]>> GetCountActiveAlarmsByDates(GetCountActiveAlarmsByDatesQuery content);
    }
}
