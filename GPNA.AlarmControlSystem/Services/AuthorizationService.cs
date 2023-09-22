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
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly AuthenticationStateProvider _authenticationState;
    private readonly ILogger<AuthorizationService> _logger;

    public AuthorizationService(IAlarmControlSystemApiBroker apiBroker, ProtectedSessionStorage sessionStorage,
        AuthenticationStateProvider authenticationState, ILogger<AuthorizationService> logger,
        AuthenticationStateProvider provider)
    {
        _apiBroker = apiBroker;
        _sessionStorage = sessionStorage;
        _authenticationState = authenticationState;
        _logger = logger;
    }

    public async Task<string> GetAndSaveApiToken()
    {
        var authState = await _authenticationState.GetAuthenticationStateAsync();
        var login = authState?.User?.Identity?.Name;

        var query = HttpUtility.ParseQueryString("");
        query["Login"] = login;
        query["JwtSecretKey"] = JwtOptions.KEY;
        var uri = query.ToString();

        var response = await _apiBroker.Get<JwtDto>("/api/Authorization/GetBearerToken?" + uri);

        if (response.Success)
        {
            await _sessionStorage.SetAsync("apiToken", response.Payload.Token);
            return response.Payload.Token;
        }
        
        _logger.LogError($"Failed to get bearer token for login {login}");

        return string.Empty;
    }

    public async Task<bool> GetAuthorizationStatus()
    {
        try
        {
            await _apiBroker.SetAuthenticationHeader();
            
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