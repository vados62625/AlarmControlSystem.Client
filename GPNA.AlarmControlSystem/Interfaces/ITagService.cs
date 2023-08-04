using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface ITagService : ICrudBase<TagDto>
{
    Task<Result<PageableCollectionDto<TagDto>>> GetCollection(int workStationId);
}