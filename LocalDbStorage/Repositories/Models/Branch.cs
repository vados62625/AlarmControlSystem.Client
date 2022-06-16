using System;
using System.Collections.Generic;

namespace GPNA.ACSAPI.Repositories.Models
{
    public partial class Branch
    {
        public Branch()
        {
            WorkStations = new HashSet<WorkStation>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Del { get; set; }

        public virtual ICollection<WorkStation> WorkStations { get; set; }
    }
}
