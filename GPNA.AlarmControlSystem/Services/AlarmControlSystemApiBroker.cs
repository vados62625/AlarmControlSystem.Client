using System.Net.Http.Headers;
using Blazored.Toast.Services;
using GPNA.RestClient.Exceptions;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Brokers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace GPNA.AlarmControlSystem.Services;

public interface IAlarmControlSystemApiBroker : IApiBrokerBase
{
    public Task SetAuthenticationHeader();
}

public class AlarmControlSystemApiBroker : ApiBrokerBase, IAlarmControlSystemApiBroker
{
    private readonly IToastService _toastService;

    private readonly ProtectedSessionStorage _sessionStorage;

    private readonly ILogger<AlarmControlSystemApiBroker> _logger;

    public AlarmControlSystemApiBroker(HttpClient httpClient, IToastService toastService,
        ProtectedSessionStorage sessionStorage,
        ILogger<AlarmControlSystemApiBroker> logger)
        : base(httpClient)
    {
        _toastService = toastService;
        _sessionStorage = sessionStorage;
        _logger = logger;

        Task.Run(() => SetAuthenticationHeader().Wait());
    }

    public async Task SetAuthenticationHeader()
    {
        var apiTokenResult = await _sessionStorage.GetAsync<string>("apiToken");

        var apiToken = apiTokenResult.Value;

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
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
}