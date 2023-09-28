using System.Security.Claims;
using System.Web;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Options;
using GPNA.RestClient.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace GPNA.AlarmControlSystem.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IAlarmControlSystemApiBroker _apiBroker;
    private readonly ILogger<AuthorizationService> _logger;

    public AuthorizationService(IAlarmControlSystemApiBroker apiBroker, ILogger<AuthorizationService> logger)
    {
        _apiBroker = apiBroker;
        _logger = logger;
    }

    public async Task<bool> GetAuthorizationStatus()
    {
        try
        {
            var response = await _apiBroker.Get<object>("/api/Authorization/AuthorizationStatus");

            return response.Success;
        }
        catch (ProblemDetailsException e)
        {
            _logger.LogDebug("AuthorizationStatus Failed");
            return false;
        }
    }
}