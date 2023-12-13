using GPNA.AlarmControlSystem.Models.Dto.Tag;
using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.Queries
{
    public class ExportTagsQuery : GetTagsListQuery
    {
        public ExportDocumentType DocumentType { get; set; }
    }
}
