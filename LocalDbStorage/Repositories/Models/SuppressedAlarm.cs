using System;
using System.Collections.Generic;

namespace GPNA.ACSAPI.Repositories.Models
{
    public partial class SuppressedAlarm
    {
        public int IdWorkStation { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Duration { get; set; }
        public string TagName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public float? AlarmLmit { get; set; }
        public string? Value { get; set; }
        public string? Comment { get; set; }
    }
}
