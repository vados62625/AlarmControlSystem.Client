using System.Security.Policy;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class WorkStationService : CrudBase<WorkStationDto>, IWorkStationService
{
    private const string URL = "api/WorkStations";
    public WorkStationService(IAlarmControlSystemApiBroker broker) : base(broker, URL)
    { }
    
    public Task<Result<ICollection<WorkstationMainPageDto>>> GetWorkstationsWithStatistics(GetAlarmsCountForFieldQuery query)
    {
        return ApiBroker.Get<ICollection<WorkstationMainPageDto>>($"{URL}/GetAlarmsCountForField", query);
    }
}