using GPNA.AlarmControlSystem.Models.Dto.Tag;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface ITagService : ICrudBase<TagDto>
{
}