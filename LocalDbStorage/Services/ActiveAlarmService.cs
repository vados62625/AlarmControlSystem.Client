using AutoMapper;
using LocalDbStorage.Extentions;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Repositories.Models;
using LocalDbStorage.Repositories.Models.Enum;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ActiveAlarmDto = LocalDbStorage.Dto.ActiveAlarmDto;

namespace LocalDbStorage.Services;

public class ActiveAlarmService : IActiveAlarmService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ActiveAlarmService> _log;
    private readonly IBufferAlarmService _bufferAlarmService;
    private readonly Mapper _mapper;
    DateTime _dateTime = new DateTime();
    List<ActiveAlarmDto> _alarms = new List<ActiveAlarmDto>();

    public ActiveAlarmService(IHttpClientFactory httpClientFactory, ILogger<ActiveAlarmService> log, IBufferAlarmService bufferAlarmService)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(ActiveAlarmService));
        _log = log;
        _bufferAlarmService = bufferAlarmService;

        var config = new MapperConfiguration(cfg => cfg.CreateMap<BufferAlarm, ActiveAlarmDto>());
        _mapper = new Mapper(config);
    }

    /// <summary>
    /// Получить все Активные аварии
    /// </summary>
    /// <returns></returns>
    public async Task<List<ActiveAlarmDto>> GetAllAlarms()
    {
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
                    var item = itemGroup.Find(c => c.EventAlarm == EventAlarm.Normalization);

                    var duration = DateTime.Now - bufferAlarm.DateTime;

                    if (item == null && duration.Days > 1)
                    {
                        var activeAlarm = _mapper.Map<BufferAlarm, ActiveAlarmDto>(bufferAlarm);
                        activeAlarm.Duration = duration;
                        _alarms.Add(activeAlarm);
                    }
                }
            }
        }
        return _alarms;
    }

    /// <summary>
    /// Получить все сигнализации
    /// </summary>
    /// <returns></returns>
    public async Task<List<ActiveAlarmDto>> GetAllActiveAlarms(int idWorkStation)
    {
        return await _httpClient.Get<List<ActiveAlarmDto>>($"/api/BufferAlarm/Get/ActiveAlarmsByIdWorkSt?idWorkStation={idWorkStation}");
    }

    /// <summary>
    /// Изменить запись в БД
    /// </summary>
    /// <returns></returns>
    public async Task UpdateAlarm(ActiveAlarmDto activeAlarm, int id)
    {
        try
        {
            await _httpClient.Put<ActiveAlarmDto>($"/api/ActiveAlarm/{id}", activeAlarm);
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
    public async Task<List<ActiveAlarmDto>> GetScopeAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetAllAlarms();
        //result = result.Where(c => c.StartDate >= startDate && c.StartDate <= endDate).ToList();
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
            //var d = result.FindAll(c => c.StartDate.Date == s.Key.Date);
            //_count[s.Key] = d.Count;
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