using Blazored.Toast.Services;
using GPNA.RestClient.Interfaces.Brokers;
using GPNA.RestClient.Models;
using GPNA.RestClient.Services.Brokers;

namespace GPNA.AlarmControlSystem.Services;

public interface IAlarmControlSystemApiBroker : IApiBrokerBase
{
}

public class AlarmControlSystemApiBroker : ApiBrokerBase, IAlarmControlSystemApiBroker
{
    private readonly IToastService _toastService;
    private readonly ILogger<AlarmControlSystemApiBroker> _logger;
    public AlarmControlSystemApiBroker(HttpClient httpClient, IToastService toastService, ILogger<AlarmControlSystemApiBroker> logger)
        : base(httpClient)
    {
        _toastService = toastService;
        _logger = logger;
    }
    
    public async Task<Result<T>> Get<T>(string uri, object? content = null)
    {
        try
        {
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