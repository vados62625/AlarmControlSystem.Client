using System.Security.Principal;
using System.Text;
using GPNA.AlarmControlSystem.Application.Dto;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;
using Microsoft.AspNetCore.Authorization;

namespace GPNA.AlarmControlSystem.Services
{
    public class IncomingAlarmService : CrudBase<IncomingAlarmDto>, IIncomingAlarmService
    {
        readonly IAlarmControlSystemApiBroker _apiBroker;
        private readonly IHttpContextAccessor _httpContextAccessor;

        const string URL = "api/IncomingAlarms";

        public IncomingAlarmService(IAlarmControlSystemApiBroker apiBroker, IHttpContextAccessor httpContextAccessor) : base(apiBroker, URL)
        {
            _apiBroker = apiBroker;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Получить количество входящие сигнализаций за дату в рамках заданного периода
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<Result<CountAlarmsOnDate[]>> GetCountIncomingAlarmsByDates(
            GetIncomingAlarmsByDatesQuery content)
        {
            return await _apiBroker.Get<CountAlarmsOnDate[]>($"{URL}/GetCountIncomingAlarmsByDates", content);
        }

        public Task<Result<CountAlarmsOnPriority[]>> GetCountIncomingAlarmsByPriorities(
            GetIncomingAlarmsByDatesQuery content)
        {
            return _apiBroker.Get<CountAlarmsOnPriority[]>($"{URL}/GetCountIncomingAlarmsByPriorities", content);
        }

        public async Task<Result<IncomingAlarmDto[][]>> GetAlarmsPerDate(GetIncomingAlarmsByDatesQuery content)
        {
            return await _apiBroker.Get<IncomingAlarmDto[][]>($"{URL}/GetIncomingAlarmsPerDate", content);
        }

        public Task<Result<Dictionary<DateTimeOffset, IncomingAlarmDto[]>>> GetCountInHour(GetIncomingAlarmsByDatesQuery content)
        {
            return _apiBroker.Get<Dictionary<DateTimeOffset, IncomingAlarmDto[]>>($"{URL}/GetCountIncomingAlarmsInHour", content);
        }
    }
}