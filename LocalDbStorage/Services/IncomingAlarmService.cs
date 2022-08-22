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
}