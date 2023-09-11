using System.ComponentModel.DataAnnotations;

namespace GPNA.AlarmControlSystem.Models.Dto.Email;

public class SendEmailQuery
{
    /// <summary>
    /// Получатель
    /// </summary>
    [Required]
    public string Reciever { get; set; }

    /// <summary>
    /// Отправитель
    /// </summary>
    [Required]
    public string Sender { get; set; }

    /// <summary>
    /// Id сигнализации
    /// </summary>
    [Required]
    public int BufferAlarmId { get; set; }
}