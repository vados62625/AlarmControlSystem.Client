using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Services
{
    public class SuppressedAlarmService : ISuppressedAlarmService
    {
        IAlarmControlSystemApiBroker _apiBroker;

        const string URL = "api/SuppressedAlarms";
        public SuppressedAlarmService(IAlarmControlSystemApiBroker apiBroker)
        {
            _apiBroker = apiBroker;
        }

        /// <summary>
        /// Получить подавленные сигнализации в рамках заданного периода
        /// </summary>
        /// <returns></returns>
        public async Task<Result<PageableCollectionDto<SuppressedAlarmDto>>> GetSuppressedAlarmsByDates(GetSuppressedAlarmsListQuery content)
        {
            return await _apiBroker.Get<PageableCollectionDto<SuppressedAlarmDto>>($"{URL}", content);
        }

        /// <summary>
        /// Получить количество подавленных сигнализаций за дату в рамках заданного периода
        /// </summary>
        /// <returns></returns>
        public async Task<Result<CountAlarmsOnDate[]>> GetCountSuppressedAlarmsByDates(GetCountSuppressedAlarmsByDatesQuery content)
        {
            return await _apiBroker.Get<CountAlarmsOnDate[]>($"{URL}/GetCountSuppressedAlarmsByDates", content);
        }
    }
}
