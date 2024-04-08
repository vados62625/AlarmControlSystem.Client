using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.User;
using GPNA.RestClient.Interfaces.Brokers;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IUserService : ICrudBase<UserDto>
{ }