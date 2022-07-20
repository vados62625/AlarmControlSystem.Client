using GPNA.ACSAPI.Repositories.Base;

namespace GPNA.ACSAPI.Repositories.Models
{
    /// <summary>
    /// Активные сигнализации
    /// </summary>
    public partial class ActiveAlarm : EntityBase
    {
        /// <summary>
        /// Id рабочей станции
        /// </summary>
        public int IdWorkStation { get; set; }

        /// <summary>
        /// Начало аварии
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Конец аварии
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Длительность
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// Имя тега
        /// </summary>
        public string TagName { get; set; } = null!;

        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public StateType State { get; set; }

        /// <summary>
        /// Приоритет
        /// </summary>
        public PriorityType Priority { get; set; }

        /// <summary>
        /// Предел
        /// </summary>
        public string? AlarmLimit { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public string? Comment { get; set; }

        public virtual WorkStation WorkStation { get; set; } = null!;
    }
}
