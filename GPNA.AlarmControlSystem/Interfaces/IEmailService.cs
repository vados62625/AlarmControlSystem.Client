using GPNA.AlarmControlSystem.Models.Dto.Email;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IEmailService
{
    Task<bool> SendMessage(SendEmailQuery query);
    Task<bool> SendTaskMessage(SendTaskMessageCommand command);
}