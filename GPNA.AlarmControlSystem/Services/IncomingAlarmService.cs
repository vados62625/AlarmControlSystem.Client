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
        public async Task<Result<CountAlarmsOnDate[]>> GetCountIncomingAlarmsByDates(GetCountIncomingAlarmsByDatesQuery content)
        {
            return await _apiBroker.Get<CountAlarmsOnDate[]>($"{URL}/GetCountIncomingAlarmsByDates", content);
        }


        ////TODO оптимизировать, вынести в АПИ
        //public async Task<List<List<IncomingAlarmDto>>> GetAlarmsPerDate(int idWorkStation, DateTime startDate, DateTime endDate)
        //{
        //    var result = await GetIncomingAlarmsByDates(new GetIncomingAlarmsListQuery());

        //    if (result.Success)
        //    {
        //        return result.Payload.Items.GroupBy(c => new { c.DateTimeActivation.Date, c.TagName, c.State })
        //            .Select(group => group.ToList())
        //            .ToList();
        //    }

        //    return default;
        //}

        ////TODO оптимизировать
        //public async Task<Dictionary<DateTime, List<IncomingAlarmDto>>> GetCountInHour(int idWorkStation, DateTime startDate, DateTime endDate)
        //{
        //    var result = await GetIncomingAlarmsByDates(new GetIncomingAlarmsListQuery());

        //    var dictionaryCount = new Dictionary<DateTime, List<IncomingAlarmDto>>();

        //    if (result.Success)
        //    {
        //        var dateTimes = Enumerable
        //            .Range(0, (int)(endDate - startDate).TotalHours + 1)
        //            .Select(x => startDate.AddHours(x))
        //            .ToArray();

        //        foreach (var dateTime in dateTimes)
        //        {
        //            var list = result.Payload.Items.FindAll(c => c.DateTimeActivation > dateTime & c.DateTimeActivation < dateTime.AddHours(1));
        //            dictionaryCount.TryAdd(dateTime, list);
        //        }
        //    }

        //    return dictionaryCount;
        //}
    }
}
