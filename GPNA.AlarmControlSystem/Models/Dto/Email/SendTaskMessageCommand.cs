using System.ComponentModel.DataAnnotations;

namespace GPNA.AlarmControlSystem.Models.Dto.Email;

public class SendTaskMessageCommand
{
    /// <summary>
    /// Получатель
    /// </summary>
    [Required]
    public string MessageText { get; set; }

    /// <summary>
    /// Отправитель
    /// </summary>
    [Required]
    public string Sender { get; set; }

    /// <summary>
    /// Id сигнализации
    /// </summary>
    [Required]
    public int TagTaskId { get; set; }
}