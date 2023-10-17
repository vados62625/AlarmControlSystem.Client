using GPNA.AlarmControlSystem.Application.Dto;

namespace GPNA.AlarmControlSystem.Models.Dto.Workstation;

public class WorkstationMainPageDto
{
    public string Name { get; set; }
    public Dictionary<DateTimeOffset, CountAlarmsOnPriority[]> CountAlarmsOnPrioritiesMap { get; set; } = new();
    public bool Fire { get; set; } = false;
    public bool Gas { get; set; } = false;
    public int TotalAlarmsCount { get; set; }
    public int TotalUniqueAlarmsCount { get; set; }
}