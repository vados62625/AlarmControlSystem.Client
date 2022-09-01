using LocalDbStorage.Dto;
using LocalDbStorage.Extentions;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using Microsoft.Extensions.Logging;

namespace LocalDbStorage.Services;

public class IncomingAlarmService : IIncomingAlarmService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IncomingAlarmService> _log;
    public const int Day = 24;
    
    public IncomingAlarmService(IHttpClientFactory httpClientFactory, ILogger<IncomingAlarmService> log)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(IncomingAlarmService));
        _log = log;
    }
    /// <summary>
    /// Получить все аварии
    /// </summary>
    /// <returns></returns>
    public async Task<List<IncomingAlarm>> GetAllAlarms()
    {
        var result = await _httpClient.Get<List<IncomingAlarm>>("/api/IncomingAlarm?itemsOnPage=0&page=1");
        return result;
    }

    public async Task<List<List<IncomingAlarm>>> GetCountInDate(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var alarms = await GetScopeAlarms(idWorkStation, startDate, endDate);

        List<List<IncomingAlarm>> groups = alarms.GroupBy(c => new { c.Date.Date, c.TagName })
            .Select(group => group.ToList())
            .ToList();

        return groups;
    }

    public async Task<Dictionary<DateTime, List<IncomingAlarm>>> GetCountInHour(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var alarms = await GetScopeAlarms(idWorkStation, startDate, endDate);

        var dictionary = new Dictionary<DateTime, List<IncomingAlarm>>();

        var test = alarms.OrderBy(c => c.Date).ToList();

        var first = test.First().Date;
        var last = test.Last().Date;

        var tm = first.Date;
        var oneH = new TimeSpan(1,0,0);

        while (tm < last)
        {
            var list = alarms.FindAll(c => c.Date > tm & c.Date < tm + oneH);
            tm += oneH;

            if(list.Count > 0)
                dictionary.TryAdd(tm, list);

        }

        return dictionary;
    }

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public async Task<List<IncomingAlarm>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await _httpClient.Get<List<IncomingAlarm>>($"/api/IncomingAlarm/ScopeByDatesAndIdWorkSt?idWorkStation={idWorkStation}&startDate={startDate:yyyy-MM-dd hh:mm:ss}&endDate={endDate:yyyy-MM-dd hh:mm:ss}") ?? new List<IncomingAlarm>();
        return result;
    }
    
    public async Task<Dictionary<DateTime, double>> GetAverageAlarmsByDates(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetScopeAlarms(idWorkStation, startDate, endDate);

        Dictionary<DateTime, double> _count = new Dictionary<DateTime, double>();

        var groupDateTime = result.GroupBy(g => g.Date);

        groupDateTime = groupDateTime.OrderBy(u => u.Key).ToList();

        foreach (var g in groupDateTime)
        {
            _count.Add(g.Key, g.Count() / Day);
        }

        return _count;

    }

    //public async Task<Dictionary<DateTime, double>> GetAlarmsInHour(int idWorkStation, DateTime startDate, DateTime endDate)
    //{
    //    var result = await GetScopeAlarms(idWorkStation, startDate, endDate);

    //    Dictionary<DateTime, double> _count = new Dictionary<DateTime, double>();

    //    var groupDateTime = result.GroupBy(g => g.Date.Hour);

    //    groupDateTime = groupDateTime.OrderBy(u => u.Key).ToList();

    //    foreach (var g in groupDateTime)
    //    {
    //        _count.Add(g.Key, g.Count());
    //    }

    //    return _count;

    //}
}