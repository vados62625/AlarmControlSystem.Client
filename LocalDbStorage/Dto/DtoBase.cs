using LocalDbStorage.Repositories.Models.Enum;

namespace LocalDbStorage.Dto
{
    public class DtoBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id рабочей станции
        /// </summary>
        public int IdWorkStation { get; set; }

        /// <summary>
        /// Начало сигнализации
        /// </summary>
        public DateTime DateTime { get; set; }

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
    }
}
