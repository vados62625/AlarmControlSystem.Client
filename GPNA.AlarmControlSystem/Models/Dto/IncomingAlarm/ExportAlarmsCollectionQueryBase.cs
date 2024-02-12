using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm
{
    public class ExportAlarmsCollectionQueryBase : GetAlarmsCollectionQueryBase
    {
        public ExportDocumentType DocumentType { get; set; }
    }
}
