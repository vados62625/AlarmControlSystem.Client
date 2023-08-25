using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces
{
    public interface IIncomingAlarmService
    {
        Task<Result<PageableCollectionDto<IncomingAlarmDto>>> GetIncomingAlarmsByDates(object? content = null);
    }
}
