using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class UserDto : EntityDtoBase
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Права доступа
        /// </summary>
        public AccessType Access { get; set; }
    }
}
