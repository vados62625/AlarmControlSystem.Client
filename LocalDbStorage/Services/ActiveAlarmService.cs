using AutoMapper;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using Microsoft.Extensions.Logging;
using ActiveAlarmDto = LocalDbStorage.Dto.ActiveAlarmDto;

namespace LocalDbStorage.Services;

public class ActiveAlarmService : IActiveAlarmService
{
    private readonly ILogger<ActiveAlarmService> _log;
    private readonly IBufferAlarmService _bufferAlarmService;
    private readonly Mapper _mapActiveToBuffer;

    public ActiveAlarmService(ILogger<ActiveAlarmService> log, IBufferAlarmService bufferAlarmService)
    {
        _log = log;
        _bufferAlarmService = bufferAlarmService;
        _mapActiveToBuffer = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ActiveAlarmDto, BufferAlarm>()));
    }

    /// <summary>
    /// Получить все Активные аварии
    /// </summary>
    /// <returns></returns>
    public async Task<List<ActiveAlarmDto>> GetAllAlarms()
    {
        var result =  await _bufferAlarmService.GetAllActiveAlarms();
        return result;
    }

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    public async Task UpdateAlarm(ActiveAlarmDto activeAlarm)
    {
        var bufferAlarm = _mapActiveToBuffer.Map<ActiveAlarmDto, BufferAlarm>(activeAlarm);
        await _bufferAlarmService.UpdateBufferAlarm(bufferAlarm);
    }

    /// <summary>
    /// Возвращает все записи по Id рабочей станции за определенный период
    /// </summary>
    /// <param name="idWorkStation"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public async Task<List<ActiveAlarmDto>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetAllAlarms();
        result = result.Where(c => c.DateTime >= startDate && c.DateTime <= endDate).ToList();
        return result;
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
            var d = result.FindAll(c => c.DateTime.Date == s.Key.Date);
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

    private TimeSpan GetDuration(DateTime startDateTime, DateTime enDateTime)
    {
        return enDateTime - startDateTime;
    }
}