using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Services;

public class FieldService : IFieldService
{
    private const string Url = "api/Fields";

    private readonly IAlarmControlSystemApiBroker _broker;

    public FieldService(IAlarmControlSystemApiBroker broker)
    {
        _broker = broker;
    }

    public Task<Result<FieldMainPageDto>> GetField(GetAlarmsCountForFieldQuery query)
    {
        return _broker.Get<FieldMainPageDto>($"{Url}/GetAlarmsCountForField", query);
    }
}