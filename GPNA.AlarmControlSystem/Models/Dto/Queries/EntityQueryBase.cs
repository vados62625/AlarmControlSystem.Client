namespace GPNA.AlarmControlSystem.Models.Dto.Queries
{
    public class EntityQueryBase
    {
        public int WorkStationId { get; set; }
        public virtual DateTimeOffset? DateTimeStart { get; set; }
        public virtual DateTimeOffset? DateTimeEnd { get; set; }
        public int? Page { get; set; }
        public int? ItemsOnPage { get; set; }
        public string? Suggest { get; set; }
    }
}
