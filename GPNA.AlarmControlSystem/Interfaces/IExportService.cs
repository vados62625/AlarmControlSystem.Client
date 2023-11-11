using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportIncomingAlarms(ExportAlarmsCollectionQuery query);
    Task<byte[]> ExportActiveAlarmsReport(ExportAlarmsCollectionQuery query);
}