using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.KpiSettings;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class TagTableSettingsService : CrudBase<TagTableSettingsDto>
{
    IAlarmControlSystemApiBroker _apiBroker;

    const string URL = "api/TagTableSettings";
    
    public TagTableSettingsService(IAlarmControlSystemApiBroker apiBroker) : base(apiBroker, URL)
    {
        _apiBroker = apiBroker;
    }
    
    public Task<Result<TagTableSettingsDto>> GetSettings(int workStationId)
    {
        return _apiBroker.Get<TagTableSettingsDto>(URL, new { WorkStationId = workStationId });
    }
}