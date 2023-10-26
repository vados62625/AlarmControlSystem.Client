using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IWorkStationService : ICrudBase<WorkStationDto>
{
    Task<Result<ICollection<WorkstationMainPageDto>>> GetWorkstationsWithStatistics(GetAlarmsCountForFieldQuery query);
}