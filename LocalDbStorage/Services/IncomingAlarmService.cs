using AutoMapper;
using LocalDbStorage.Dto;
using LocalDbStorage.Extentions;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using LocalDbStorage.Repositories.Models.Enum;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace LocalDbStorage.Services;

public class IncomingAlarmService : IIncomingAlarmService
{
    private readonly ILogger<IncomingAlarmService> _log;
    private readonly IBufferAlarmService _bufferAlarmService;
    private readonly Mapper _mapBufferToIncoming;
    private readonly Mapper _mapIncomingToBuffer;
    List<IncomingAlarmDto> _alarms = new List<IncomingAlarmDto>();
    public const int Day = 24;
    
    public IncomingAlarmService(IBufferAlarmService bufferAlarmService, ILogger<IncomingAlarmService> log)
    {
        _log = log;
        _bufferAlarmService = bufferAlarmService;
        _mapBufferToIncoming = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<BufferAlarm, IncomingAlarmDto>()));
        _mapIncomingToBuffer = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<IncomingAlarmDto, BufferAlarm>()));
    }
    
    /// <summary>
    /// Получить все входящие аварии
    /// </summary>
    /// <returns></returns>
    public async Task<List<IncomingAlarmDto>> GetAllAlarms()
    {
        _alarms.Clear();
        
        var bufferAlarms = await _bufferAlarmService.GetAllAlarms();
// сортируем по имени тега 
        var groupsOfTags = bufferAlarms
            .GroupBy(u => u.TagName)
            .Select(g => g.ToList())
            .ToList();

        // перебираем лист по тегу
        foreach (var group in groupsOfTags)
        {
            var itemGroup = group.OrderBy(u => u.DateTime).ToList();

            foreach (var bufferAlarm in itemGroup)
            {
                if (bufferAlarm.EventAlarm == (int)EventAlarm.Activation)
                {
                    var incomingAlarm = _mapBufferToIncoming.Map<BufferAlarm, IncomingAlarmDto>(bufferAlarm);
                    _alarms.Add(incomingAlarm);
                }
            }
        }
        return _alarms;
    }

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    public async Task UpdateAlarm(IncomingAlarmDto incomingAlarm)
    {
        var bufferAlarm = _mapIncomingToBuffer.Map<IncomingAlarmDto, BufferAlarm>(incomingAlarm);
        await _bufferAlarmService.UpdateAlarm(bufferAlarm);
    }

    public async Task<List<List<IncomingAlarmDto>>> GetAlarmsPerDate(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var alarms = await GetScopeAlarms(idWorkStation, startDate, endDate);

        List<List<IncomingAlarmDto>> groups = alarms.GroupBy(c => new { c.DateTime.Date, c.TagName })
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
            var d = result.FindAll(c => c.DateTime.Date == s.Key.Date);
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