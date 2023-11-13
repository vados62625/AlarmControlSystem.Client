using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm
{
    public class ExportIncomingAlarmsByDatesQuery : GetIncomingAlarmsByDatesQuery
    {
        public ExportDocumentType DocumentType { get; set; }
    }
}
