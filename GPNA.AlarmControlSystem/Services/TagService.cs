using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class TagService : CrudBase<TagDto>, ITagService
{
    private const string URL = "api/Tags";
    public TagService(IAlarmControlSystemApiBroker broker) : base(broker, URL)
    { }
    
    public async Task<Result<PageableCollectionDto<TagDto>>> GetCollection()
    {
        return await ApiBroker.Get<PageableCollectionDto<TagDto>>("api/Tags");
    }
}