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

    public async Task<byte[]> ExportIncomingAlarms(ExportIncomingAlarmsByDatesQuery query)
    {
        return await _apiBroker.GetFile($"{URL}/ExportIncomingAlarms", query);
    }

    public async Task<byte[]> ExportActiveAlarms(ExportIncomingAlarmsByDatesQuery query)
    {
        return await _apiBroker.GetFile($"{URL}/ExportActiveAlarms", query);
    }

    public async Task<byte[]> ExportImitatedAlarms(ExportIncomingAlarmsByDatesQuery query)
    {
        return await _apiBroker.GetFile($"{URL}/ExportImitatedAlarms", query);
    }

    public async Task<byte[]> ExportSuppressedAlarms(ExportIncomingAlarmsByDatesQuery query)
    {
        return await _apiBroker.GetFile($"{URL}/ExportSuppressedAlarms", query);
    }

    public async Task<byte[]> ExportActiveAlarmsReport(ExportIncomingAlarmsByDatesQuery query)
    {
        return await _apiBroker.GetFile($"{URL}/ExportActiveAlarmsReport", query);
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