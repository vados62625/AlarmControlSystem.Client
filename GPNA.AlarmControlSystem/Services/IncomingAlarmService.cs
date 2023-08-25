using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services
{
    public class IncomingAlarmService : IIncomingAlarmService
    {
        IAlarmControlSystemApiBroker _apiBroker;

        const string URL = "api/IncomingAlarms";
        public IncomingAlarmService(IAlarmControlSystemApiBroker apiBroker)
        {
            _apiBroker = apiBroker;
        }


        /// <summary>
        /// Получить входящие сигнализации в рамках заданного периода
        /// </summary>
        /// <returns></returns>
        public async Task<Result<PageableCollectionDto<IncomingAlarmDto>>> GetIncomingAlarmsByDates(object? content = null)
        {
            return await _apiBroker.Get<PageableCollectionDto<IncomingAlarmDto>>($"{URL}/GetAllIncomingAlarms", content);
        }
    }
}
