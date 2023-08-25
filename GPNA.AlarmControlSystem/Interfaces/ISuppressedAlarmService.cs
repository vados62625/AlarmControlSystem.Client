using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface ISuppressedAlarmService
    {
        Task<Result<PageableCollectionDto<SuppressedAlarmDto>>> GetSuppressedAlarmsByDates(
            GetSuppressedAlarmsListQuery content);

        Task<Result<CountAlarmsOnDate[]>> GetCountSuppressedAlarmsByDates(GetCountSuppressedAlarmsByDatesQuery content);
    }
}
