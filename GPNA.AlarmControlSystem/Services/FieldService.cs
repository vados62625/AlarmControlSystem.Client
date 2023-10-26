using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class FieldService : CrudBase<FieldDto>, IFieldService
{
    private const string Url = "api/Fields";
    
    public FieldService(IAlarmControlSystemApiBroker broker) : base(broker, Url)
    { }
}