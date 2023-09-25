using System.Net.Http.Headers;
using System.Web;
using Blazored.Toast.Services;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Options;
using GPNA.RestClient.Exceptions;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Brokers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace GPNA.AlarmControlSystem.Services;

public interface IAlarmControlSystemApiBroker : IApiBrokerBase
{
}

public class AlarmControlSystemApiBroker : ApiBrokerBase, IAlarmControlSystemApiBroker
{
    private readonly IToastService _toastService;
    
    private readonly AuthenticationStateProvider _authenticationState;

    private readonly ILogger<AlarmControlSystemApiBroker> _logger;

    public AlarmControlSystemApiBroker(HttpClient httpClient, IToastService toastService,
        AuthenticationStateProvider authenticationState,
        ILogger<AlarmControlSystemApiBroker> logger)
        : base(httpClient)
    {
        _toastService = toastService;
        _authenticationState = authenticationState;
        _logger = logger;

        var apiToken = Task.Run(GetApiToken).Result;

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
    }

    public async Task<Result<T>> Get<T>(string uri, object? content = null)
    {
        try
        {
            var result = await base.Get<T>(uri, content);

            if (result.Success)
                return result;

            _toastService.ShowError($"Доступ запрещен");

            return new Result<T>("Доступ запрещен");
        }
        catch (Exception ex) when (
            ex is System.Net.Sockets.SocketException ||
            ex is System.Net.Http.HttpRequestException
        )
        {
            _toastService.ShowError($"Нет связи с сервером. Ошибка получения данных с источника {uri.Split("?")[0]}");

            _logger.LogError(ex.Message);

            return new Result<T>("Нет связи с сервером");
        }
    }

    private async Task<string> GetApiToken()
    {
        var authState = await _authenticationState.GetAuthenticationStateAsync();
        var login = authState?.User?.Identity?.Name;

        var query = HttpUtility.ParseQueryString("");
        query["Login"] = login;
        query["JwtSecretKey"] = JwtOptions.KEY;
        var uri = query.ToString();

        var response = await base.Get<JwtDto>("/api/Authorization/GetBearerToken?" + uri);

        if (response.Success)
        {
            return response.Payload.Token;
        }

        _logger.LogError($"Failed to get bearer token for login {login}");

        return string.Empty;
    }
}