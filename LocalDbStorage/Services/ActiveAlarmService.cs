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
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    public async Task UpdateAlarm(ActiveAlarm activeAlarm, int id)
    {
        try
        {
            await _httpClient.Put<ActiveAlarm>($"/api/ActiveAlarm/{id}", activeAlarm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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
        var result = await GetAllAlarms();
        result = result.Where(c => c.StartDate >= startDate && c.StartDate <= endDate).ToList();
        return result;

        // TODO временно endDate в контроллере не используется
        //var result = await _httpClient.Get<List<ActiveAlarm>>($"/api/ActiveAlarm/ScopeByDatesAndIdWorkSt?idWorkStation={idWorkStation}&startDate={startDate:yyyy-MM-ddTHH:mm:ss}&endDate={endDate:yyyy-MM-ddTHH:mm:ss}") ?? new List<ActiveAlarm>();
        //return result;
    }

    public async Task<Dictionary<DateTime, int>> CountOfActiveAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetAllAlarms();

        Dictionary<DateTime, int> _count = new Dictionary<DateTime, int>();

        var itemTimeSpan = endDate - startDate;
        if (itemTimeSpan.Days > 0)
        {
            for (int i = 0; i <= itemTimeSpan.Days; i++)    
            {
                TimeSpan ts = TimeSpan.FromDays(i);
                _count.Add(startDate + ts, 0);
            }
        }

        foreach (var s in _count)
        {
            var d = result.FindAll(c => c.StartDate.Date == s.Key.Date);
            _count[s.Key] = d.Count;
        }

        return _count;
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