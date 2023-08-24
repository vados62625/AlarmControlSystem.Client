using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services
{
    public class IncomingAlarmService : CrudBase<IncomingAlarmDto>, IIncomingAlarmService
    {
        const string URL = "api/IncomingAlarms";
        public IncomingAlarmService(IAlarmControlSystemApiBroker broker) : base(broker, URL)
        {

        }
    }
}
