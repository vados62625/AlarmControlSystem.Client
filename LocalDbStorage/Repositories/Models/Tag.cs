using GPNA.ACSAPI.Repositories.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GPNA.ACSAPI.Repositories.Models
{

    public partial class Tag : ParentEntityBase
    {
        public string Position { get; set; } = null!;
        public string TagName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public float AlarmLimit { get; set; }
        public float Scale { get; set; }
        public string Consequence { get; set; } = null!;
        public string ActionFieldOperator { get; set; } = null!;
        public string ActionFieldArm { get; set; } = null!;
        public string Inform { get; set; } = null!;
        public string ReactionTime { get; set; } = null!;
    }
}
