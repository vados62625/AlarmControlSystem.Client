using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.TagTask;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class TagTaskService : CrudBase<TagTaskDto>, ITagTaskService
{
    private const string URL = "api/TagTasks";
    private readonly IAlarmControlSystemApiBroker _apiBroker;

    public TagTaskService(IAlarmControlSystemApiBroker broker) : base(broker, URL)
    {
        _apiBroker = broker;
    }

    public async Task<Result<TagTaskDto>> CreateTagTask(int alarmId)
    {
        return await _apiBroker.Post<TagTaskDto>(URL, new { bufferAlarmId = alarmId });
    }
    
    public async Task<Result<TagTaskDto[]>> CreateTagTasks(int[] alarmIds)
    {
        return await _apiBroker.Post<TagTaskDto[]>($"{URL}/CreateManyTasks", new { bufferAlarmIds = alarmIds });
    }

    public async Task<Result<TagTaskDto>> UpdateTagTask(UpdateTagTaskCommand command)
    {
        return await _apiBroker.Patch<TagTaskDto>(URL, command);
    }

    public async Task<Result<TagTasksCollection>> GetTagTasksCollection(GetTagTasksListQuery query)
    {
        return await _apiBroker.Get<TagTasksCollection>(URL, query);
    }
}