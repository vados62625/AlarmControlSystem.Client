using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportIncomingAlarms(ExportIncomingAlarmsByDatesQuery query);
}