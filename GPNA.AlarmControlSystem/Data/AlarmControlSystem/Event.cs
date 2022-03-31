using System;
using System.Collections.Generic;

namespace GPNA.AlarmControlSystem.Data.AlarmControlSystem
{
    public partial class Event
    {
        public DateTime? LastDate { get; set; }
        public TimeSpan? LastTime { get; set; }
        public string? TagName { get; set; }
        public string? Description { get; set; }
        public int? Priority { get; set; }
        public double? AlarmLmit { get; set; }
        public string? Value { get; set; }
        public string? Comment { get; set; }
        public bool? Test { get; set; }
        public int? Count { get; set; }
    }
}
