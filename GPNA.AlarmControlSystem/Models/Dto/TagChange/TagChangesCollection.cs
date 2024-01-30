namespace GPNA.AlarmControlSystem.Application.Dto.TagChange;

public class TagChangesCollection
{
    public TagChangeDto[] Items { get; set; } = Array.Empty<TagChangeDto>();
    
    public int PagesCount { get; set; } = 1;

    public int TotalCount { get; set; }

    public int IncorrectDescription { get; set; }
    
    public int IncorrectPriority { get; set; }
    
    public int IncorrectAlarmLimit { get; set; }
}