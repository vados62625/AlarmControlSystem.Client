namespace GPNA.AlarmControlSystem.Models.Dto;

public class PageableCollectionDto<T>
{
    public List<T> Items { get; set; } = new List<T>();
    
    public int TotalCount { get; set; }
}
