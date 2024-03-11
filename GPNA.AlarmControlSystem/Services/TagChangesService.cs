using GPNA.AlarmControlSystem.Application.Dto.TagChange;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.TagChange;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class TagChangesService : CrudBase<TagChangeDto>, ITagChangesService
{
    private const string URL = "api/TagChanges";
    private readonly IAlarmControlSystemApiBroker _apiBroker;

    public TagChangesService(IAlarmControlSystemApiBroker broker) : base(broker, URL)
    {
        _apiBroker = broker;
    }
    
    public async Task<Result<TagChangeDto[]>> CreateTagChange(AddTagChangeCommand command)
    {
        return await _apiBroker.Post<TagChangeDto[]>(URL, command);
    }
    
    public async Task<Result<TagChangeDto[]>> CreateTagChanges(AddTagChangeListCommand command)
    {
        return await _apiBroker.Post<TagChangeDto[]>($"{URL}/CreateMany", command);
    }

    public async Task<Result<TagChangesCollection>> GetTagChangesCollection(GetTagChangesListQuery query)
    {
        return await _apiBroker.Get<TagChangesCollection>(URL, query);
    }
}