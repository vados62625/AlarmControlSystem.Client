using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportIncomingAlarms(ExportIncomingAlarmsByDatesQuery query);
    Task<byte[]> ExportActiveAlarms(ExportIncomingAlarmsByDatesQuery query);
    Task<byte[]> ExportImitatedAlarms(ExportIncomingAlarmsByDatesQuery query);
    Task<byte[]> ExportSuppressedAlarms(ExportIncomingAlarmsByDatesQuery query);
    Task<byte[]> ExportActiveAlarmsReport(ExportIncomingAlarmsByDatesQuery query);
}