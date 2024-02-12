using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.Queries;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportIncomingAlarms(ExportAlarmsCollectionQueryBase queryBase);
    Task<byte[]> ExportActiveAlarms(ExportAlarmsCollectionQueryBase queryBase);
    Task<byte[]> ExportImitatedAlarms(ExportAlarmsCollectionQueryBase queryBase);
    Task<byte[]> ExportSuppressedAlarms(ExportAlarmsCollectionQueryBase queryBase);
    Task<byte[]> ExportActiveAlarmsReport(ExportAlarmsCollectionQueryBase queryBase);
    Task<byte[]> ExportTags(ExportTagsQuery query);
    Task<byte[]> ExportTagTasks(ExportTagTasksQuery query);
}