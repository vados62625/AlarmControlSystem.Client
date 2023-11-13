using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.KpiSettings;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class ReportSettingsService : CrudBase<ReportSettingsDto>
{
    IAlarmControlSystemApiBroker _apiBroker;

    const string URL = "api/ReportSettings";
    
    public ReportSettingsService(IAlarmControlSystemApiBroker apiBroker) : base(apiBroker, URL)
    {
        _apiBroker = apiBroker;
    }
    
    public Task<Result<ReportSettingsDto>> GetSettings(int workStationId)
    {
        return _apiBroker.Get<ReportSettingsDto>(URL, new { WorkStationId = workStationId });
    }
}