using AccessType = GPNA.AlarmControlSystem.Domain.Enums.AccessType;

namespace GPNA.AlarmControlSystem.Models.Dto.User
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class UserDto : Domain.Dto.Base.EntityDtoBase
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Права доступа
        /// </summary>
        public AccessType Access { get; set; }

        public override bool Equals(object? obj)
        {
            var value = (UserDto)obj;
            if (value == null)
            {
                return false;
            }

            return Login == value.Login &&
                   Access == value.Access &&
                   Id == value.Id;
        }
    }
}
