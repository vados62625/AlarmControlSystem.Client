using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Services.Brokers;

namespace GPNA.AlarmControlSystem.Services;

public interface IAlarmControlSystemApiBroker : IApiBrokerBase
{
}

public class AlarmControlSystemApiBroker : ApiBrokerBase, IAlarmControlSystemApiBroker
{
    public AlarmControlSystemApiBroker(HttpClient httpClient)
        : base(httpClient)
    {
    }
}