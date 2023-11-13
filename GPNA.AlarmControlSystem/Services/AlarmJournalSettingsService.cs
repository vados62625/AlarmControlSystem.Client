using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.KpiSettings;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Base;

namespace GPNA.AlarmControlSystem.Services;

public class AlarmJournalSettingsService : CrudBase<AlarmJournalSettingsDto>
{
    IAlarmControlSystemApiBroker _apiBroker;

    const string URL = "api/AlarmJournalSettings";
    
    public AlarmJournalSettingsService(IAlarmControlSystemApiBroker apiBroker) : base(apiBroker, URL)
    {
        _apiBroker = apiBroker;
    }

    public Task<Result<AlarmJournalSettingsDto>> GetSettings(int workStationId)
    {
        return _apiBroker.Get<AlarmJournalSettingsDto>(URL, new { WorkStationId = workStationId });
    }
}