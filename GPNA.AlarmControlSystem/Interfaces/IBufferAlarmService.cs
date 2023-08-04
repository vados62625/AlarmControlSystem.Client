using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IBufferAlarmService
{
    Task<Result<PageableCollectionDto<ActiveAlarmDto>>> GetAllActiveAlarms(int workStationId);
}