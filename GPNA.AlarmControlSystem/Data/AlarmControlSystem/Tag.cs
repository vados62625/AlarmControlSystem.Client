using System;
using System.Collections.Generic;

namespace GPNA.AlarmControlSystem.Data.AlarmControlSystem
{
    public partial class Tag
    {
        public string? Position { get; set; }
        public string? TagName { get; set; }
        public string? Description { get; set; }
        public string? Unit { get; set; }
        public string? State { get; set; }
        public string? Priority { get; set; }
        public double? AlarmLimit { get; set; }
        public double? Scale { get; set; }
        public string? Consequence { get; set; }
        public string? ActionFieldOperator { get; set; }
        public string? ActionFieldArm { get; set; }
        public string? Inform { get; set; }
        public string? ReactionTime { get; set; }
    }
}
