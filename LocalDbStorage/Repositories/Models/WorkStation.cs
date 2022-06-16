using System;
using System.Collections.Generic;

namespace GPNA.ACSAPI.Repositories.Models
{
    public partial class WorkStation
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string? Name { get; set; }
        public bool Del { get; set; }

        public virtual Branch Branch { get; set; } = null!;
    }
}
