using LocalDbStorage.Extentions;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using Microsoft.Extensions.Logging;

namespace LocalDbStorage.Services;

public class SuppressedAlarmService : ISuppressedAlarmService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SuppressedAlarmService> _log;
    public const int Day = 24;
    
    public SuppressedAlarmService(IHttpClientFactory httpClientFactory, ILogger<SuppressedAlarmService> log)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(SuppressedAlarmService));
        _log = log;
    }

    /// <summary>
    /// Получить все Подавленные аварии
    /// </summary>
    /// <returns></returns>
    public async Task<List<SuppressedAlarm>> GetAllAlarms()
    {
        var result = await _httpClient.Get<List<SuppressedAlarm>>("/api/SuppressedAlarm?itemsOnPage=0&page=1");
        return result;
    }

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    public async Task UpdateAlarm(SuppressedAlarm suppressedAlarm, int id)
    {
        try
        {
            await _httpClient.Put<SuppressedAlarm>($"/api/SuppressedAlarm/{id}", suppressedAlarm);
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
    public async Task<List<SuppressedAlarm>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await _httpClient.Get<List<SuppressedAlarm>>($"/api/SuppressedAlarm/ScopeByDatesAndIdWorkSt?idWorkStation={idWorkStation}&startDate={startDate:yyyy-MM-ddTHH:mm:ss}&endDate={endDate:yyyy-MM-ddTHH:mm:ss}") ?? new List<SuppressedAlarm>();
        return result;
    }

    public async Task<Dictionary<DateTime, int>> CountOfSuppressedAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetScopeAlarms(idWorkStation, startDate, endDate);

        return Count(result.Select(c => c.Date).ToList());
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