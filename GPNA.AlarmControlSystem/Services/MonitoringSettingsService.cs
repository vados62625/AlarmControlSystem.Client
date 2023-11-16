using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.KpiSettings;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class MonitoringSettingsService : CrudBase<MonitoringSettingsDto>
{
    IAlarmControlSystemApiBroker _apiBroker;

    const string URL = "api/MonitoringSettings";
    
    public MonitoringSettingsService(IAlarmControlSystemApiBroker apiBroker) : base(apiBroker, URL)
    {
        _apiBroker = apiBroker;
    }
    
    public Task<Result<MonitoringSettingsDto>> GetSettings(int workStationId)
    {
        return _apiBroker.Get<MonitoringSettingsDto>(URL, new { WorkStationId = workStationId });
    }
}