namespace GPNA.AlarmControlSystem.Models.Dto
{
    public class EntityQueryBase
    {
        public int WorkStationId { get; set; }
        public virtual DateTimeOffset? CreatedAtFrom { get; set; }
        public virtual DateTimeOffset? CreatedAtTo { get; set; }
        public int? Page { get; set; }
        public int? ItemsOnPage { get; set; }
        public string? Suggest { get; set; }
    }
}
