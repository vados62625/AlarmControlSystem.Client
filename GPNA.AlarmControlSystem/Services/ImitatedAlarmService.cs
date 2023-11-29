using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.ImitatedAlarm;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services
{
    public class ImitatedAlarmService : CrudBase<ImitatedAlarmDto>, IImitatedAlarmService
    {
        IAlarmControlSystemApiBroker _apiBroker;

        const string URL = "api/ImitatedAlarms";
        public ImitatedAlarmService(IAlarmControlSystemApiBroker apiBroker) : base(apiBroker, URL)
        {
            _apiBroker = apiBroker;
        }

        /// <summary>
        /// Получить количество активных сигнализаций за дату в рамках заданного периода
        /// </summary>
        /// <returns></returns>
        public async Task<Result<CountAlarmsOnDate[]>> GetCountImitatedAlarmsByDates(GetCountImitatedAlarmsByDatesQuery content)
        {
            return await _apiBroker.Get<CountAlarmsOnDate[]>($"{URL}/GetCountImitatedAlarmsByDates", content);
        }
        
        /// <summary>
        /// Получить количество активных сигнализаций за дату в рамках заданного периода
        /// </summary>
        /// <returns></returns>
        public async Task<Result<AlarmsCollection<ImitatedAlarmDto>>> GetImitatedAlarmsPerDate(GetIncomingAlarmsByDatesQuery content)
        {
            return await _apiBroker.Get<AlarmsCollection<ImitatedAlarmDto>>($"{URL}/GetImitatedAlarmsPerDate", content);
        }
    }
}
