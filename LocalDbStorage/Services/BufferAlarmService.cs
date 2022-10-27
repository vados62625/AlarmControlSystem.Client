using LocalDbStorage.Dto;
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
    /// Получить все активные сигнализации
    /// </summary>
    /// <returns></returns>
    public async Task<List<ActiveAlarmDto>> GetAllActiveAlarms()
    {
        List<ActiveAlarmDto> activeAlarms;

        if (_memoryCache.TryGetValue("ActiveAlarms", out activeAlarms))
            return activeAlarms;

        activeAlarms = await _httpClient.Get<List<ActiveAlarmDto>>("/api/BufferAlarm/GetAllActiveAlarms");

        _memoryCache.Set("ActiveAlarms", activeAlarms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));

        return activeAlarms;
    }

    /// <summary>
    /// Получить все входящие сигнализации
    /// </summary>
    /// <returns></returns>
    public async Task<List<IncomingAlarmDto>> GetAllIncomingAlarms()
    {
        List<IncomingAlarmDto> incomingAlarms;

        if (_memoryCache.TryGetValue("IncomingAlarms", out incomingAlarms))
            return incomingAlarms;

        incomingAlarms = await _httpClient.Get<List<IncomingAlarmDto>>("/api/BufferAlarm/GetAllIncomingAlarms");

        _memoryCache.Set("IncomingAlarms", incomingAlarms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));

        return incomingAlarms;
    }

    /// <summary>
    /// Получить все подавленные сигнализации
    /// </summary>
    /// <returns></returns>
    public async Task<List<SuppressedAlarmDto>> GetAllSuppressedAlarms()
    {
        List<SuppressedAlarmDto> suppressedAlarms;

        if (_memoryCache.TryGetValue("SuppressedAlarms", out suppressedAlarms))
            return suppressedAlarms;

        suppressedAlarms = await _httpClient.Get<List<SuppressedAlarmDto>>("/api/BufferAlarm/GetAllSuppressedAlarms");

        _memoryCache.Set("SuppressedAlarms", suppressedAlarms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));

        return suppressedAlarms;
    }

    /// <summary>
    /// Получить все сигнализации
    /// </summary>
    /// <returns></returns>
    public async Task<List<BufferAlarm>> GetAllBufferAlarms()
    {
        List<BufferAlarm> bufferAlarms;

        if (_memoryCache.TryGetValue("BufferAlarms", out bufferAlarms))
            return bufferAlarms;

        bufferAlarms = await _httpClient.Get<List<BufferAlarm>>("/api/BufferAlarm?itemsOnPage=0&page=1");

        _memoryCache.Set("BufferAlarms", bufferAlarms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));

        return bufferAlarms;
    }

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    public async Task UpdateBufferAlarm(BufferAlarm bufferAlarm)
    {
        try
        {
            await _httpClient.Put<BufferAlarm>($"/api/BufferAlarm/{bufferAlarm.Id}", bufferAlarm);
            _memoryCache.Remove("BufferAlarms");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

}