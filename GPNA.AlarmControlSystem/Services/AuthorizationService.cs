using System.Security.Claims;
using System.Web;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace GPNA.AlarmControlSystem.Services;

public class AuthorizationService
{
    private readonly IAlarmControlSystemApiBroker _apiBroker;
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly AuthenticationStateProvider _authenticationState;
    private readonly ILogger<AuthorizationService> _logger;
    // private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthorizationService(IAlarmControlSystemApiBroker apiBroker, ProtectedSessionStorage sessionStorage, AuthenticationStateProvider authenticationState, ILogger<AuthorizationService> logger, AuthenticationStateProvider provider)
    {
        _apiBroker = apiBroker;
        _sessionStorage = sessionStorage;
        _authenticationState = authenticationState;
        _logger = logger;
        // _authenticationStateProvider = provider;
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

        return string.Empty;
    }
    public async Task<bool> GetAuthorizationStatus()
    {
        var response = await _apiBroker.Get<object>("/api/Authorization/AuthorizationStatus");
        
        return response.Success;
    }
}