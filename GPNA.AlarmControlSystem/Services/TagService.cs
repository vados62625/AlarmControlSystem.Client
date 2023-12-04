using GPNA.AlarmControlSystem.Application.Dto.Tag;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Tag;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class TagService : CrudBase<TagDto>, ITagService
{
    private const string URL = "api/Tags";
    private readonly IAlarmControlSystemApiBroker _apiBroker;

    public TagService(IAlarmControlSystemApiBroker broker) : base(broker, URL)
    {
        _apiBroker = broker;
    }

    public async Task<Result<TagsCollection>> GetTagsCollection(GetTagsListQuery query)
    {
        return await _apiBroker.Get<TagsCollection>(URL, query);
    }
}