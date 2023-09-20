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
    private readonly ILogger<AlarmControlSystemApiBroker> _logger;
    public AlarmControlSystemApiBroker(HttpClient httpClient, IToastService toastService, ProtectedSessionStorage sessionStorage, ILogger<AlarmControlSystemApiBroker> logger)
        : base(httpClient)
    {
        _toastService = toastService;
        _sessionStorage = sessionStorage;
        _logger = logger;
    }
    
    public async Task<Result<T>> Get<T>(string uri, object? content = null)
    {
        try
        {
            var apiToken = await _sessionStorage.GetAsync<string>("apiToken");
            
            if (!string.IsNullOrEmpty(apiToken.Value))
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken.Value);
                // HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");
            }
            
            return await base.Get<T>(uri, content);
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