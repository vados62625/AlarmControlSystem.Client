using GPNA.ACSAPI.Repositories.Base;
using LocalDbStorage.Repositories.Models.Enum;

namespace GPNA.ACSAPI.Repositories.Models;

/// <summary>
/// Пользователь
/// </summary>
public partial class User : EntityBase
{
    /// <summary>
    /// Логин
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Права доступа
    /// </summary>
    public Access? Access { get; set; }
}