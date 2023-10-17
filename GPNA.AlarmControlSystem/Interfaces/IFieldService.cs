using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IFieldService
{
    Task<Result<FieldMainPageDto>> GetField(GetAlarmsCountForFieldQuery query);
}