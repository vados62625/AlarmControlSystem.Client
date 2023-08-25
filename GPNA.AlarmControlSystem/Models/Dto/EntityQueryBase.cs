namespace GPNA.AlarmControlSystem.Models.Dto
{
    public class EntityQueryBase
    {
        public int WorkStationId { get; set; }
        public virtual DateTimeOffset? ActivationFrom { get; set; }
        public virtual DateTimeOffset? ActivationTo { get; set; }
        public int? Page { get; set; }
        public int? ItemsOnPage { get; set; }
        public string? Suggest { get; set; }
    }
}
