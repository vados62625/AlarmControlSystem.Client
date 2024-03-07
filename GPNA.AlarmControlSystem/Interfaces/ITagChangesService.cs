using GPNA.AlarmControlSystem.Application.Dto.Tag;
using GPNA.AlarmControlSystem.Application.Dto.TagChange;
using GPNA.AlarmControlSystem.Models.Dto.TagChange;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface ITagChangesService : ICrudBase<TagChangeDto>
{
    Task<Result<TagChangeDto[]>> CreateTagChange(int tagId, int initiatorId, int executorId, string? comment = null);
    Task<Result<TagChangeDto[]>> CreateTagChanges(int[] tagIds, int initiatorId, int executorId, string? comment = null);
    Task<Result<TagChangesCollection>> GetTagChangesCollection(GetTagChangesListQuery query);
}