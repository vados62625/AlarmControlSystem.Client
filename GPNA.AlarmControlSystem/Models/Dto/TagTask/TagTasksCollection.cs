using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.TagTask;

public class TagTasksCollection
{
    public IEnumerable<TagTaskDto> Items { get; set; }

    public int TotalCount { get; set; }

    public Dictionary<PriorityType, int> CountByPriority { get; set; }

    public Dictionary<StateType, int> CountByState { get; set; }
    
    public bool Fire { get; set; }

    public bool Gas { get; set; }

    /// <summary>
    /// Количество страниц
    /// </summary>
    public int PagesCount { get; set; } = 1;
}