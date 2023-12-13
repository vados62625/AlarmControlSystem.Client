using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.Queries;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportIncomingAlarms(ExportIncomingAlarmsByDatesQuery query);
    Task<byte[]> ExportActiveAlarms(ExportIncomingAlarmsByDatesQuery query);
    Task<byte[]> ExportImitatedAlarms(ExportIncomingAlarmsByDatesQuery query);
    Task<byte[]> ExportSuppressedAlarms(ExportIncomingAlarmsByDatesQuery query);
    Task<byte[]> ExportActiveAlarmsReport(ExportIncomingAlarmsByDatesQuery query);
    Task<byte[]> ExportTags(ExportTagsQuery query);
    Task<byte[]> ExportTagTasks(ExportTagTasksQuery query);
}