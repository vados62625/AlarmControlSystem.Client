using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Services;

public class BufferAlarmService : IBufferAlarmService
{
    protected readonly IAlarmControlSystemApiBroker _apiBroker;

    const string URL = "api/BufferAlarms";

    public BufferAlarmService(IAlarmControlSystemApiBroker apiBroker)
    {
        _apiBroker = apiBroker;
    }


    //TODO возможно не будет работать, надо проверить
    /// <summary>
    /// Добавить коммент к сигнализации
    /// </summary>
    /// <param name="id"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<Result<BufferAlarmDto>> AddComment(AddCommentInAlarmCommand content)
    {
        return await _apiBroker.Patch<BufferAlarmDto>($"{URL}/AddCommentInAlarm", content);
    }
    
    //TODO возможно не будет работать, надо проверить
    /// <summary>
    /// Изменить статус сигнализации
    /// </summary>
    /// <param name="id"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<Result<BufferAlarmDto>> ChangeStatus(ChangeStatusAlarmCommand content)
    {
        return await _apiBroker.Patch<BufferAlarmDto>($"{URL}/ChangeStatusAlarm", content);
    }
}