using LocalDbStorage.Dto;
using LocalDbStorage.Extentions;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

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

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    public async Task UpdateAlarm(IncomingAlarm incomingAlarm, int id)
    {
        try
        {
            await _httpClient.Put<IncomingAlarm>($"/api/IncomingAlarm/{id}", incomingAlarm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task<List<List<IncomingAlarm>>> GetAlarmsPerDate(int idWorkStation, DateTime startDate, DateTime endDate)
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

        var dictionaryCount = new Dictionary<DateTime, List<IncomingAlarm>>();

        if (alarms.Count == 0)
            return dictionaryCount;

        var _startDate = startDate;
        var _endDate = endDate;
        var oneHoure = new TimeSpan(1, 0, 0);

        while (_startDate < _endDate)
        {
         
            var list = alarms.FindAll(c => c.Date > _startDate & c.Date < _startDate + oneHoure);
            _startDate += oneHoure;

            dictionaryCount.TryAdd(_startDate, list);
        }
        return dictionaryCount;
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
        var result = await _httpClient.Get<List<IncomingAlarm>>($"/api/IncomingAlarm/ScopeByDatesAndIdWorkSt?idWorkStation={idWorkStation}&startDate={startDate:yyyy-MM-ddTHH:mm:ss}&endDate={endDate:yyyy-MM-ddTHH:mm:ss}");
        return result;
    }
    
    public async Task<Dictionary<DateTime, double>> GetAverageAlarmsByDates(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetScopeAlarms(idWorkStation, startDate, endDate);

        Dictionary<DateTime, double> _count = new Dictionary<DateTime, double>();

        var itemTimeSpan = endDate - startDate;
        if (itemTimeSpan.Days > 0)
        {
            for (int i = 0; i <= itemTimeSpan.Days; i++)
            {
                TimeSpan ts = TimeSpan.FromDays(i);
                var date = startDate + ts;
                _count.Add(date.Date, 0);
            }
        }

        foreach (var s in _count)
        {
            var d = result.FindAll(c => c.Date.Date == s.Key);
            _count[s.Key] = d.Count / Day;
        }

        //var groupDateTime = result.GroupBy(g => g.Date.Date);

        //groupDateTime = groupDateTime.OrderBy(u => u.Key).ToList();

        //foreach (var g in groupDateTime)
        //{
        //    _count[g.Key] = g.Count() / Day;
        //}

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