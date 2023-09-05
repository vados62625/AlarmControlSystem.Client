using GPNA.AlarmControlSystem.Application.Dto;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services
{
    public class IncomingAlarmService : CrudBase<IncomingAlarmDto>, IIncomingAlarmService
    {
        readonly IAlarmControlSystemApiBroker _apiBroker;

        const string URL = "api/IncomingAlarms";

        public IncomingAlarmService(IAlarmControlSystemApiBroker apiBroker) : base(apiBroker, URL)
        {
            _apiBroker = apiBroker;
        }

        /// <summary>
        /// Получить количество входящие сигнализаций за дату в рамках заданного периода
        /// </summary>
        /// <returns></returns>
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

        public async Task<IncomingAlarmDto[][]> GetAlarmsPerDate(GetIncomingAlarmsByDatesQuery content)
        {
            var result = await this.GetCollection(new GetIncomingAlarmsListQuery()
                { WorkStationId = content.WorkStationId, ActivationFrom = content.ActivationFrom, ActivationTo = content.ActivationTo });

            if (result.Success)
            {
                return result.Payload.Items.GroupBy(c => new { c.DateTimeActivation.Date, c.TagName, c.State })
                    .Select(group => group.ToArray())
                    .ToArray();
            }

            return default;
        }

        public async Task<Result<Dictionary<DateTimeOffset, IncomingAlarmDto[]>>> GetCountInHour(GetIncomingAlarmsByDatesQuery content)
        {
            return await _apiBroker.Get<Dictionary<DateTimeOffset, IncomingAlarmDto[]>>($"{URL}/GetCountIncomingAlarmsInHour", content);
        }
    }
}