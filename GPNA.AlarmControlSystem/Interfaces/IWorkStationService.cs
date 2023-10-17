using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.RestClient.Interfaces.Brokers;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IWorkStationService : ICrudBase<WorkStationDto>
{ }