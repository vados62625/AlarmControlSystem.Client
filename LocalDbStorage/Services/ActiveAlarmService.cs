using LocalDbStorage.Extentions;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using Microsoft.Extensions.Logging;

namespace LocalDbStorage.Services;

public class ActiveAlarmService : IActiveAlarmService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ActiveAlarmService> _log;
    public const int Day = 24;
    
    public ActiveAlarmService(IHttpClientFactory httpClientFactory, ILogger<ActiveAlarmService> log)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(ActiveAlarmService));
        _log = log;
    }

    /// <summary>
    /// Получить все Активные аварии
    /// </summary>
    /// <returns></returns>
    public async Task<List<ActiveAlarm>> GetAllAlarms()
    {
        var result = await _httpClient.Get<List<ActiveAlarm>>("/api/ActiveAlarm?itemsOnPage=0&page=1");
        return result;
    }

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public async Task<List<ActiveAlarm>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await _httpClient.Get<List<ActiveAlarm>>($"/api/ActiveAlarm/ScopeByDatesAndIdWorkSt?idWorkStation={idWorkStation}&startDate={startDate:yyyy-MM-dd hh:mm:ss}&endDate={endDate:yyyy-MM-dd hh:mm:ss}") ?? new List<ActiveAlarm>();
        return result;
    }

    public async Task<Dictionary<DateTime, int>> CountOfActiveAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetScopeAlarms(idWorkStation, startDate, endDate);

        Dictionary<DateTime, int> _count = new Dictionary<DateTime, int>();

        var test = endDate - startDate;
        if (test.Days > 0)
        {
            for (int i = 0; i <= test.Days; i++)
            {
                TimeSpan ts = TimeSpan.FromDays(i);
                _count.Add(startDate + ts, 0);
            }
        }

        foreach (var v in _count)
        {
            foreach (var s in result)
            {
                if (s.StartDate >= v.Key && s.EndDate > v.Key)
                {
                    var value = _count[v.Key];
                    _count[v.Key] = ++value;
                }
            }
        }

        return Count(result.Select(c => c.StartDate).ToList());
    }

    private Dictionary<DateTime, int> Count(List<DateTime> list)
    {
        Dictionary<DateTime, int> _count = new Dictionary<DateTime, int>();

        var groupDateTime = list.GroupBy(g => g.Date);

        groupDateTime = groupDateTime.OrderBy(u => u.Key).ToList();

        foreach (var g in groupDateTime)
        {
            _count.Add(g.Key, g.Count());
        }

        return _count;
    }
}