using GPNA.AlarmControlSystem.Models.Dto.TagTask;
using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.Queries
{
    public class ExportTagTasksQuery : GetTagTasksListQuery
    {
        public ExportDocumentType DocumentType { get; set; }
    }
}
