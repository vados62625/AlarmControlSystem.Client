using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.KpiSettings;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class TaskSettingsService : CrudBase<TaskSettingsDto>
{
    IAlarmControlSystemApiBroker _apiBroker;

    const string URL = "api/TaskSettings";
    
    public TaskSettingsService(IAlarmControlSystemApiBroker apiBroker) : base(apiBroker, URL)
    {
        _apiBroker = apiBroker;
    }
    
    public Task<Result<TaskSettingsDto>> GetSettings(int workStationId)
    {
        return _apiBroker.Get<TaskSettingsDto>(URL, new { WorkStationId = workStationId });
    }
}