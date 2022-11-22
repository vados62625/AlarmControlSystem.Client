using LocalDbStorage.Repositories.Models.Enum;

namespace LocalDbStorage.Repositories.Base
{
    public class EntityBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Статус сигнализации
        /// </summary>
        public StatusAlarm StatusAlarm { get; set; }
    }
}
