using AutoMapper;
using LocalDbStorage.Dto;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using Microsoft.Extensions.Logging;

namespace LocalDbStorage.Services;

public class IncomingAlarmService : IIncomingAlarmService
{
    private readonly ILogger<IncomingAlarmService> _log;
    private readonly IBufferAlarmService _bufferAlarmService;
    private readonly Mapper _mapIncomingToBuffer;
    public const int Day = 24;
    
    public IncomingAlarmService(IBufferAlarmService bufferAlarmService, ILogger<IncomingAlarmService> log)
    {
        _log = log;
        _bufferAlarmService = bufferAlarmService;
        _mapIncomingToBuffer = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<IncomingAlarmDto, BufferAlarm>()));
    }
    
    /// <summary>
    /// Получить все входящие аварии
    /// </summary>
    /// <returns></returns>
    public async Task<List<IncomingAlarmDto>> GetAllAlarms()
    {
        return await _bufferAlarmService.GetAllIncomingAlarms();
    }

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    public async Task UpdateAlarm(IncomingAlarmDto incomingAlarm)
    {
        var bufferAlarm = _mapIncomingToBuffer.Map<IncomingAlarmDto, BufferAlarm>(incomingAlarm);
        await _bufferAlarmService.UpdateBufferAlarm(bufferAlarm);
    }

    public async Task<List<List<IncomingAlarmDto>>> GetAlarmsPerDate(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var alarms = await GetScopeAlarms(idWorkStation, startDate, endDate);

        List<List<IncomingAlarmDto>> groups = alarms.GroupBy(c => new { c.DateTime.Date, TagName = c.TagName,  c.State})
            .Select(group => group.ToList())
            .ToList();

        return groups;
    }

    public async Task<Dictionary<DateTime, List<IncomingAlarmDto>>> GetCountInHour(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var alarms = await GetScopeAlarms(idWorkStation, startDate, endDate);

        var dictionaryCount = new Dictionary<DateTime, List<IncomingAlarmDto>>();

        if (alarms.Count == 0)
            return dictionaryCount;

        var _startDate = startDate;
        var _endDate = endDate;
        var oneHoure = new TimeSpan(1, 0, 0);

        while (_startDate < _endDate)
        {
         
            var list = alarms.FindAll(c => c.DateTime > _startDate & c.DateTime < _startDate + oneHoure);
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
    public async Task<List<IncomingAlarmDto>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetAllAlarms();
        result = result.Where(c => c.DateTime >= startDate && c.DateTime <= endDate).ToList();
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
            var count = Convert.ToDouble((result.FindAll(c => c.DateTime.Date == s.Key.Date)).Count);
            _count[s.Key] = Convert.ToInt32(count / Day);
        }

        //var groupDateTime = result.GroupBy(g => g.Date.Date);

        //groupDateTime = groupDateTime.OrderBy(u => u.Key).ToList();

        //foreach (var g in groupDateTime)
        //{
        //    _count[g.Key] = g.Count() / Day;
        //}

        return _count;
    }

}