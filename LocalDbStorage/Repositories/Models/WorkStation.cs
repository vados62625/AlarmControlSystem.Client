using GPNA.ACSAPI.Repositories.Base;

namespace GPNA.ACSAPI.Repositories.Models
{
    /// <summary>
    /// Рабочая станция
    /// </summary>
    public class WorkStation : EntityBase
    {
        public WorkStation()
        {
            ActiveAlarms = new HashSet<ActiveAlarm>();
            IncomingAlarms = new HashSet<IncomingAlarm>();
            SuppressedAlarms = new HashSet<SuppressedAlarm>();
        }

        /// <summary>
        /// Id Дочернего общества
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Метка удаления записи
        /// </summary>
        public bool Del { get; set; }

        public virtual Branch Branch { get; set; } = null!;
        public virtual ICollection<ActiveAlarm> ActiveAlarms { get; set; }
        public virtual ICollection<IncomingAlarm> IncomingAlarms { get; set; }
        public virtual ICollection<SuppressedAlarm> SuppressedAlarms { get; set; }
    }
}
