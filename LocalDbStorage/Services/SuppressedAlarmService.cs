using AutoMapper;
using LocalDbStorage.Dto;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using Microsoft.Extensions.Logging;

namespace LocalDbStorage.Services;

public class SuppressedAlarmService : ISuppressedAlarmService
{
    private readonly ILogger<SuppressedAlarmService> _log;
    private readonly IBufferAlarmService _bufferAlarmService;
    private readonly Mapper _mapBufferToSuppressed;
    private readonly Mapper _mapSuppressedToBuffer;
    public const int Day = 24;
    List<SuppressedAlarmDto> _alarms = new List<SuppressedAlarmDto>();
    
    public SuppressedAlarmService(IBufferAlarmService bufferAlarmService, ILogger<SuppressedAlarmService> log)
    {
        _log = log;
        _bufferAlarmService = bufferAlarmService;
        _mapBufferToSuppressed = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<BufferAlarm, SuppressedAlarmDto>()));
        _mapSuppressedToBuffer = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<SuppressedAlarmDto, BufferAlarm>()));
    }

    /// <summary>
    /// Получить все Подавленные аварии
    /// </summary>
    /// <returns></returns>
    public async Task<List<SuppressedAlarmDto>> GetAllAlarms()
    {
        return await _bufferAlarmService.GetAllSuppressedAlarms();
    }

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    public async Task UpdateAlarm(SuppressedAlarmDto activeAlarm)
    {
        var bufferAlarm = _mapSuppressedToBuffer.Map<SuppressedAlarmDto, BufferAlarm>(activeAlarm);
        await _bufferAlarmService.UpdateBufferAlarm(bufferAlarm);
    }

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public async Task<List<SuppressedAlarmDto>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetAllAlarms();
        result = result.Where(c => c.DateTime >= startDate && c.DateTime <= endDate).ToList();
        return result;
    }

    public async Task<Dictionary<DateTime, int>> CountOfSuppressedAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var alarms = await GetScopeAlarms(idWorkStation, startDate, endDate);

        var dictionaryCount = new Dictionary<DateTime, int>();

        if (alarms.Count == 0)
            return dictionaryCount;

        var _date = startDate;
        var _endDate = endDate;
        var oneDay = new TimeSpan(1,0,0,0);

        while (_date <= _endDate)
        {

            var list = alarms.FindAll(c => c.DateTime > _date & c.DateTime < _date + oneDay);

            dictionaryCount.TryAdd(_date, list.Count);

            _date += oneDay;
        }
        return dictionaryCount;


        //return Count(alarms.Select(c => c.Date).ToList());
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