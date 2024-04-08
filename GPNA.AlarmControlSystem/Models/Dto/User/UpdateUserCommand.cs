using System.ComponentModel.DataAnnotations;
using GPNA.AlarmControlSystem.Domain.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.User;
public class UpdateUserCommand
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public int Id { get; set; }
    
    /// <summary>
    /// Права доступа
    /// </summary>
    public AccessType Access { get; set; }
}
