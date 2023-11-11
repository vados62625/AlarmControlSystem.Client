using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm
{
    public class ExportAlarmsCollectionQuery : GetAlarmsCollectionQuery
    {
        public ExportDocumentType DocumentType { get; set; }
    }
}
