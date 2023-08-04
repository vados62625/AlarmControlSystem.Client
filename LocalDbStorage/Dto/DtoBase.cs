namespace LocalDbStorage.Dto
{
    public class DtoBase
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Начало сигнализации
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Имя тега
        /// </summary>
        public string TagName { get; set; }

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

        /// <summary>
        /// Статус сигнализации
        /// </summary>
        public StatusAlarm StatusAlarm { get; set; }

        /// <summary>
        /// Идентификатор в Источнике
        /// </summary>
        public string IdInSourse { get; set; }
    }
}
