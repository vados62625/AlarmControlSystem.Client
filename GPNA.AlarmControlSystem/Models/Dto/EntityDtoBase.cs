
namespace GPNA.AlarmControlSystem.Models.Dto
{
    public abstract class EntityDtoBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Дата/Время создания
        /// </summary>
        public virtual DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Дата/Время последнего обновления
        /// </summary>
        public virtual DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// Дата/Время удаления
        /// </summary>
        public virtual DateTimeOffset? DeletedAt { get; set; }

    }
}