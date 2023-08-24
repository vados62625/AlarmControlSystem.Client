using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Services;

public class BufferAlarmService : IBufferAlarmService
{
    protected readonly IAlarmControlSystemApiBroker _apiBroker;

    public BufferAlarmService(IAlarmControlSystemApiBroker apiBroker)
    {
        _apiBroker = apiBroker;
    }
    
    /// <summary>
    /// Получить все активные сигнализации по id АРМ
    /// </summary>
    /// <param name="workStationId"></param>
    /// <returns></returns>
    public async Task<Result<PageableCollectionDto<ActiveAlarmDto>>> GetAllActiveAlarms(int workStationId)
    {
        return await _apiBroker.Get<PageableCollectionDto<ActiveAlarmDto>>($"api/ActiveAlarms/GetAllActiveAlarms/{workStationId}");
    }

    /// <summary>
    /// Получить активные сигнализации в рамках заданного периода
    /// </summary>
    /// <param name="workStationId"></param>
    /// <returns></returns>
    public async Task<Result<PageableCollectionDto<ActiveAlarmDto>>> GetActiveAlarmsByDates(int workStationId, DateTimeOffset dateTimeStart, DateTimeOffset dateTimeEnd)
    {
        return await _apiBroker.Get<PageableCollectionDto<ActiveAlarmDto>>($"api/ActiveAlarms/GetActiveAlarmsByDates/{workStationId}/{dateTimeStart:yyyy-MM-dd HH:mm:ss}/{dateTimeEnd:yyyy-MM-dd HH:mm:ss}");
    }

    /// <summary>
    /// Получить количество активных сигнализаций за дату в рамках заданного периода
    /// </summary>
    /// <param name="workStationId"></param>
    /// <returns></returns>
    public async Task<Result<CountAlarmsOnDate[]>> GetCountActiveAlarmsByDates(int workStationId, DateTimeOffset dateTimeStart, DateTimeOffset dateTimeEnd)
    {
        return await _apiBroker.Get<CountAlarmsOnDate[]>($"api/ActiveAlarms/GetCountActiveAlarmsByDates/{workStationId}/{dateTimeStart:yyyy-MM-dd HH:mm:ss}/{dateTimeEnd:yyyy-MM-dd HH:mm:ss}");
    }

    /// <summary>
    /// Получить все входящие сигнализации по id АРМ
    /// </summary>
    /// <param name="workStationId"></param>
    /// <returns></returns>
    public async Task<Result<PageableCollectionDto<IncomingAlarmDto>>> GetAllIncomingAlarms(int workStationId)
    {
        return await _apiBroker.Get<PageableCollectionDto<IncomingAlarmDto>>($"api/IncomingAlarms/GetAllIncomingAlarms/{workStationId}");
    }

    /// <summary>
    /// Получить входящие сигнализации в рамках заданного периода
    /// </summary>
    /// <param name="workStationId"></param>
    /// <returns></returns>
    public async Task<Result<PageableCollectionDto<IncomingAlarmDto>>> GetIncomingAlarmsByDates(int workStationId, DateTimeOffset dateTimeStart, DateTimeOffset dateTimeEnd)
    {
        return await _apiBroker.Get<PageableCollectionDto<IncomingAlarmDto>>($"api/IncomingAlarms/GetIncomingAlarmsByDates/{workStationId}/{dateTimeStart:yyyy-MM-dd HH:mm:ss}/{dateTimeEnd:yyyy-MM-dd HH:mm:ss}");
    }

    /// <summary>
    /// Получить количество входящие сигнализаций за дату в рамках заданного периода
    /// </summary>
    /// <param name="workStationId"></param>
    /// <returns></returns>
    public async Task<Result<CountAlarmsOnDate[]>> GetCountIncomingAlarmsByDates(int workStationId, DateTimeOffset dateTimeStart, DateTimeOffset dateTimeEnd)
    {
        return await _apiBroker.Get<CountAlarmsOnDate[]>($"api/IncomingAlarms/GetCountIncomingAlarmsByDates/{workStationId}/{dateTimeStart:yyyy-MM-dd HH:mm:ss}/{dateTimeEnd:yyyy-MM-dd HH:mm:ss}");
    }

    /// <summary>
    /// Получить все подавленные сигнализации по id АРМ
    /// </summary>
    /// <param name="workStationId"></param>
    /// <returns></returns>
    public async Task<Result<PageableCollectionDto<SuppressedAlarmDto>>> GetAllSuppressedAlarms(int workStationId)
    {
        return await _apiBroker.Get<PageableCollectionDto<SuppressedAlarmDto>>($"api/BufferAlarms/GetAllSuppressedAlarms/{workStationId}");
    }


    /// <summary>
    /// Получить подавленные сигнализации в рамках заданного периода
    /// </summary>
    /// <param name="workStationId"></param>
    /// <returns></returns>
    public async Task<Result<PageableCollectionDto<SuppressedAlarmDto>>> GetSuppressedAlarmsByDates(int workStationId, DateTimeOffset dateTimeStart, DateTimeOffset dateTimeEnd)
    {
        return await _apiBroker.Get<PageableCollectionDto<SuppressedAlarmDto>>($"api/SuppressedAlarms/GetSuppressedAlarmsByDates/{workStationId}/{dateTimeStart:yyyy-MM-dd HH:mm:ss}/{dateTimeEnd:yyyy-MM-dd HH:mm:ss}");
    }

    /// <summary>
    /// Получить количество подавленных сигнализаций за дату в рамках заданного периода
    /// </summary>
    /// <param name="workStationId"></param>
    /// <returns></returns>
    public async Task<Result<CountAlarmsOnDate[]>> GetCountSuppressedAlarmsByDates(int workStationId, DateTimeOffset dateTimeStart, DateTimeOffset dateTimeEnd)
    {
        return await _apiBroker.Get<CountAlarmsOnDate[]>($"api/SuppressedAlarms/GetCountSuppressedAlarmsByDates/{workStationId}/{dateTimeStart:yyyy-MM-dd HH:mm:ss}/{dateTimeEnd:yyyy-MM-dd HH:mm:ss}");
    }

    //TODO возможно не будет работать, надо проверить
    /// <summary>
    /// Добавить коммент к сигнализации
    /// </summary>
    /// <param name="id"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<Result<BufferAlarmDto>> AddComment(int id, string content)
    {
        return await _apiBroker.Patch<BufferAlarmDto>($"api/BufferAlarms/AddCommentInAlarm/{id}", content);
    }
    
    //TODO возможно не будет работать, надо проверить
    /// <summary>
    /// Изменить статус сигнализации
    /// </summary>
    /// <param name="id"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<Result<BufferAlarmDto>> ChangeStatus(int id, StatusAlarmType content)
    {
        return await _apiBroker.Patch<BufferAlarmDto>($"api/BufferAlarms/ChangeStatusAlarm/{id}", content);
    }

    //TODO оптимизировать, вынести в АПИ
    public async Task<List<List<IncomingAlarmDto>>> GetAlarmsPerDate(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetIncomingAlarmsByDates(idWorkStation, startDate, endDate);

        if (result.Success)
        {
            return result.Payload.Items.GroupBy(c => new { c.DateTimeActivation.Date, c.TagName, c.State })
                .Select(group => group.ToList())
                .ToList();
        }

        return default;
    }

    //TODO оптимизировать
    public async Task<Dictionary<DateTime, List<IncomingAlarmDto>>> GetCountInHour(int idWorkStation, DateTime startDate, DateTime endDate)
    {
        var result = await GetIncomingAlarmsByDates(idWorkStation, startDate, endDate);

        var dictionaryCount = new Dictionary<DateTime, List<IncomingAlarmDto>>();

        if (result.Success)
        {
            var dateTimes = Enumerable
                .Range(0, (int)(endDate - startDate).TotalHours + 1)
                .Select(x => startDate.AddHours(x))
                .ToArray();

            foreach (var dateTime in dateTimes)
            {
                var list = result.Payload.Items.FindAll(c => c.DateTimeActivation > dateTime & c.DateTimeActivation < dateTime.AddHours(1));
                dictionaryCount.TryAdd(dateTime, list);
            }
        }

        return dictionaryCount;
    }
}