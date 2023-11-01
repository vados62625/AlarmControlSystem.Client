using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Web;
using Blazored.Toast.Services;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Options;
using GPNA.RestClient.Exceptions;
using GPNA.RestClient.Extensions;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Brokers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace GPNA.AlarmControlSystem.Services;

public interface IAlarmControlSystemApiBroker : IApiBrokerBase
{
    Task<byte[]> GetFile(string uri, object? content = null);
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
    }

    public async Task<Result<T>> Get<T>(string uri, object? content = null)
    {
        return await HandleMethod(() => base.Get<T>(uri, content));
    }

    public async Task<Result<T>> Post<T>(string uri, object? content = null)
    {
        return await HandleMethod(() => base.Post<T>(uri, content));
    }

    public async Task<Result<T>> Put<T>(string uri, object? content = null)
    {
        return await HandleMethod(() => base.Put<T>(uri, content));
    }
    
    public async Task<Result<T>> Patch<T>(string uri, object? content = null)
    {
        return await HandleMethod(() => base.Patch<T>(uri, content));
    }

    public async Task<byte[]> GetFile(string uri, object? content = null)
    {
        using (var response = await HttpClient.GetAsync(uri + content.ToQueryString()))
        {
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
    }

    private async Task<Result<T>> HandleMethod<T>(Func<Task<Result<T>>> method)
    {
        try
        {
            var result = await method();

            // if (result.Error.Contains("401"))
            // {
            //     var apiToken = await GetApiToken();
            //     HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
            //     result = await method();
            // }

            if (result.Success)
                return result;

            _toastService.ShowError(result.Error.Contains("403") || result.Error.Contains("401") ? $"Доступ запрещен" : $"Ошибка запроса");

            return new Result<T>("Ошибка запроса");
        }
        catch (Exception ex) when (
            ex is System.Net.Sockets.SocketException ||
            ex is System.Net.Http.HttpRequestException
        )
        {
            _toastService.ShowError($"Нет связи с сервером");

            _logger.LogError(ex.Message);

            return new Result<T>("Нет связи с сервером");
        }
    }

    private async Task<string> GetApiToken()
    {
        var authState = await _authenticationState.GetAuthenticationStateAsync();
        var login = authState?.User?.Identity?.Name;

        if (!string.IsNullOrWhiteSpace(login))
        {
            var query = HttpUtility.ParseQueryString("");
            query["Login"] = login;
            query["JwtSecretKey"] = JwtOptions.KEY;
            var uri = query.ToString();

            var response = await base.Get<JwtDto>("/api/Authorization/GetBearerToken?" + uri);

            if (response.Success)
            {
                return response.Payload.Token;
            }
        }

        _logger.LogError($"Failed to get bearer token for login {login}");

        return string.Empty;
    }
}