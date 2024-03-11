using GPNA.AlarmControlSystem.Application.Dto.Tag;
using GPNA.AlarmControlSystem.Application.Dto.TagChange;
using GPNA.AlarmControlSystem.Models.Dto.TagChange;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface ITagChangesService : ICrudBase<TagChangeDto>
{
    Task<Result<TagChangeDto[]>> CreateTagChange(AddTagChangeCommand command);
    Task<Result<TagChangeDto[]>> CreateTagChanges(AddTagChangeListCommand command);
    Task<Result<TagChangesCollection>> GetTagChangesCollection(GetTagChangesListQuery query);
}