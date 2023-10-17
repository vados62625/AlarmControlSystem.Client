using GPNA.AlarmControlSystem.Models.Dto.Workstation;

namespace GPNA.AlarmControlSystem.Models.Dto.Field;

public class FieldMainPageDto
{
    public string Name { get; set; }
    public ICollection<WorkstationMainPageDto> Workstations { get; set; } = new HashSet<WorkstationMainPageDto>();
}