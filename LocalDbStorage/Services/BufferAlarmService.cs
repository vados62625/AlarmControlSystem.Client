using LocalDbStorage.Extentions;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace LocalDbStorage.Services;

public class BufferAlarmService : IBufferAlarmService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BufferAlarmService> _log;
    private IMemoryCache _memoryCache;
    
    public BufferAlarmService(IHttpClientFactory httpClientFactory, ILogger<BufferAlarmService> log, IMemoryCache memoryCache)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(BufferAlarmService));
        _log = log;
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Получить все сигнализации
    /// </summary>
    /// <returns></returns>
    public async Task<List<BufferAlarm>> GetAllAlarms()
    {
        List<BufferAlarm> bufferAlarms;

        if (_memoryCache.TryGetValue("BufferAlarms", out bufferAlarms))
            return bufferAlarms;

        bufferAlarms = await _httpClient.Get<List<BufferAlarm>>("/api/BufferAlarm?itemsOnPage=0&page=1");

        _memoryCache.Set("BufferAlarms", bufferAlarms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

        return bufferAlarms;
    }

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public async Task<List<BufferAlarm>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await _httpClient.Get<List<BufferAlarm>>($"/api/BufferAlarm/Get/ScopeByDateTimeAndIdWorkSt??idWorkStation={idWorkStation}&startDate={startDate:yyyy-MM-ddTHH:mm:ss}&endDate={endDate:yyyy-MM-ddTHH:mm:ss}") ?? new List<BufferAlarm>();
        return result;
    }

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    public async Task UpdateAlarm(BufferAlarm bufferAlarm, int id)
    {
        try
        {
            await _httpClient.Put<BufferAlarm>($"/api/BufferAlarm/{id}", bufferAlarm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

}