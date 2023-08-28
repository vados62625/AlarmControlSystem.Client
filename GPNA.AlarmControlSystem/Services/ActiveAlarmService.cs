using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Services
{
    public class ActiveAlarmService : IActiveAlarmService
    {
        IAlarmControlSystemApiBroker _apiBroker;

        const string URL = "api/ActiveAlarms";
        public ActiveAlarmService(IAlarmControlSystemApiBroker apiBroker)
        {
            _apiBroker = apiBroker;
        }

        /// <summary>
        /// Получить активные сигнализации в рамках заданного периода
        /// </summary>
        /// <returns></returns>
        public async Task<Result<PageableCollectionDto<ActiveAlarmDto>>> GetActiveAlarmsByDates(GetActiveAlarmsListQuery content)
        {
            return await _apiBroker.Get<PageableCollectionDto<ActiveAlarmDto>>($"{URL}", content);
        }

        /// <summary>
        /// Получить количество активных сигнализаций за дату в рамках заданного периода
        /// </summary>
        /// <returns></returns>
        public async Task<Result<CountAlarmsOnDate[]>> GetCountActiveAlarmsByDates(GetCountActiveAlarmsByDatesQuery content)
        {
            return await _apiBroker.Get<CountAlarmsOnDate[]>($"{URL}/GetCountActiveAlarmsByDates", content);
        }
    }
}
