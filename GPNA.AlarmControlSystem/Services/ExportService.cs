using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Services;

public class ExportService : IExportService
{
    protected readonly IAlarmControlSystemApiBroker _apiBroker;

    const string URL = "api/Export";

    public ExportService(IAlarmControlSystemApiBroker apiBroker)
    {
        _apiBroker = apiBroker;
    }

    public async Task<byte[]> ExportIncomingAlarms(ExportAlarmsCollectionQuery query)
    {
        return await _apiBroker.GetFile($"{URL}/ExportIncomingAlarms", query);
    }
    
    public async Task<byte[]> ExportActiveAlarmsReport(ExportAlarmsCollectionQuery query)
    {
        return await _apiBroker.GetFile($"{URL}/ExportActiveAlarmsReport", query);
    }
}