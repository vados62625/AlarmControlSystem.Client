using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.User;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class UserService : CrudBase<UserDto>, IUserService
{
    private const string URL = "api/Users";
    public UserService(IAlarmControlSystemApiBroker broker) : base(broker, URL)
    { }
}