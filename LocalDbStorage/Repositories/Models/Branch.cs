using GPNA.ACSAPI.Repositories.Base;

namespace GPNA.ACSAPI.Repositories.Models
{
    /// <summary>
    /// Кусты
    /// </summary>
    public class Branch : EntityBase
    {
        public Branch()
        {
            WorkStations = new HashSet<WorkStation>();
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Метка удаления записи
        /// </summary>
        public bool Del { get; set; }

        public virtual ICollection<WorkStation> WorkStations { get; set; }
    }
}
