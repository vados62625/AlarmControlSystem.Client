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
        return await _apiBroker.Get<PageableCollectionDto<ActiveAlarmDto>>($"api/BufferAlarms/GetAllActiveAlarms/{workStationId}");
    }
    
    /// <summary>
    /// Получить все входящие сигнализации по id АРМ
    /// </summary>
    /// <param name="workStationId"></param>
    /// <returns></returns>
    public async Task<Result<PageableCollectionDto<IncomingAlarmDto>>> GetAllIncomingAlarms(int workStationId)
    {
        return await _apiBroker.Get<PageableCollectionDto<IncomingAlarmDto>>($"api/BufferAlarms/GetAllIncomingAlarms/{workStationId}");
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
}