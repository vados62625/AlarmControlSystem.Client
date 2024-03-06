using GPNA.AlarmControlSystem.Models.Dto.Tag;
using GPNA.AlarmControlSystem.Models.Dto.TagTask;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface ITagTaskService : ICrudBase<TagTaskDto>
{
    Task<Result<TagTaskDto>> CreateTagTask(int alarmId);
    Task<Result<TagTaskDto>> UpdateTagTask(UpdateTagTaskCommand command);
    Task<Result<TagTasksCollection>> GetTagTasksCollection(GetTagTasksListQuery query);
}