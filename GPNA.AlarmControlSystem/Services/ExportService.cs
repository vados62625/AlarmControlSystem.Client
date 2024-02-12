using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
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

    public async Task<byte[]> ExportIncomingAlarms(ExportAlarmsCollectionQueryBase queryBase)
    {
        return await _apiBroker.GetFile($"{URL}/ExportIncomingAlarms", queryBase);
    }

    public async Task<byte[]> ExportActiveAlarms(ExportAlarmsCollectionQueryBase queryBase)
    {
        return await _apiBroker.GetFile($"{URL}/ExportActiveAlarms", queryBase);
    }

    public async Task<byte[]> ExportImitatedAlarms(ExportAlarmsCollectionQueryBase queryBase)
    {
        return await _apiBroker.GetFile($"{URL}/ExportImitatedAlarms", queryBase);
    }

    public async Task<byte[]> ExportSuppressedAlarms(ExportAlarmsCollectionQueryBase queryBase)
    {
        return await _apiBroker.GetFile($"{URL}/ExportSuppressedAlarms", queryBase);
    }

    public async Task<byte[]> ExportActiveAlarmsReport(ExportAlarmsCollectionQueryBase queryBase)
    {
        return await _apiBroker.GetFile($"{URL}/ExportActiveAlarmsReport", queryBase);
    }
    
    public async Task<byte[]> ExportTags(ExportTagsQuery query)
    {
        return await _apiBroker.GetFile($"{URL}/ExportTags", query);
    }
    
    public async Task<byte[]> ExportTagTasks(ExportTagTasksQuery query)
    {
        return await _apiBroker.GetFile($"{URL}/ExportTagTasks", query);
    }
}