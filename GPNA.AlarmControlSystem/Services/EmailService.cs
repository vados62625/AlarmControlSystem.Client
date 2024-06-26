﻿using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Email;

namespace GPNA.AlarmControlSystem.Services;

public class EmailService : IEmailService
{
    private readonly IAlarmControlSystemApiBroker _apiBroker;
    
    const string URL = "api/Email";

    public EmailService(IAlarmControlSystemApiBroker apiBroker)
    {
        _apiBroker = apiBroker;
    }

    public async Task<bool> SendMessage(SendEmailQuery query)
    {
        var message = await _apiBroker.Post<object>($"{URL}/SendMessage", query);
        return message.Success;
    }
    public async Task<bool> SendTaskMessage(SendTaskMessageCommand command)
    {
        var message = await _apiBroker.Post<object>($"{URL}/SendTaskMessage", command);
        return message.Success;
    }
}