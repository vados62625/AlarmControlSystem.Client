using System.Net.Http.Headers;
using Blazored.Toast.Services;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Brokers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace GPNA.AlarmControlSystem.Services;

public interface IAlarmControlSystemApiBroker : IApiBrokerBase
{
}

public class AlarmControlSystemApiBroker : ApiBrokerBase, IAlarmControlSystemApiBroker
{
    private readonly IToastService _toastService;

    private readonly ProtectedSessionStorage _sessionStorage;

    // private readonly AuthorizationService _authorizationService;
    private readonly ILogger<AlarmControlSystemApiBroker> _logger;

    public AlarmControlSystemApiBroker(HttpClient httpClient, IToastService toastService,
        ProtectedSessionStorage sessionStorage, // AuthorizationService authorizationService,
        ILogger<AlarmControlSystemApiBroker> logger)
        : base(httpClient)
    {
        _toastService = toastService;
        _sessionStorage = sessionStorage;
        // _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<Result<T>> Get<T>(string uri, object? content = null)
    {
        try
        {
            var apiTokenResult = await _sessionStorage.GetAsync<string>("apiToken");

            var apiToken = apiTokenResult.Value;
            //
            // if (!apiTokenResult.Success)
            // {
            //     apiToken = await _authorizationService.GetAndSaveApiToken();
            // }
            //
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);

            var result = await base.Get<T>(uri, content);

            if (!result.Success)
            {
                await _sessionStorage.DeleteAsync("apiToken");
            }

            return result;
        }
        catch (Exception ex) when (
            ex is System.Net.Sockets.SocketException ||
            ex is System.Net.Http.HttpRequestException
        )
        {
            _toastService.ShowError($"Нет связи с сервером. Ошибка получения данных с источника {uri}");

            _logger.LogError(ex.Message);

            return new Result<T>("Нет связи с сервером");
        }
    }
}