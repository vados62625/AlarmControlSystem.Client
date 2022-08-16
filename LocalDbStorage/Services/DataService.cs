using LocalDbStorage.Data.Extentions;
using LocalDbStorage.Data.Interfaces;
using LocalDbStorage.Repositories.Models;
using Microsoft.Extensions.Logging;

namespace LocalDbStorage.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DataService> _log;

        public DataService(IHttpClientFactory httpClientFactory, ILogger<DataService> log)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(DataService));
            _log = log;
        }

        /// <summary>
        /// Возвращает все записи по Id рабочей станции за определенный период
        /// </summary>
        /// <param name="idWorkStation"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<IncomingAlarm>> GetScopeIncomingAlarm(int idWorkStation, DateTime startDate, DateTime endDate)
        {
            var result = await _httpClient.Get<List<IncomingAlarm>>($"/api/IncomingAlarm/ScopeByDatesAndIdWorkSt?idWorkStation={idWorkStation}&startDate={startDate:yyyy-MM-dd hh:mm:ss}&endDate={endDate:yyyy-MM-dd hh:mm:ss}") ?? new List<IncomingAlarm>();
            return result;
        }

        /// <summary>
        /// Возвращает все записи по Id рабочей станции за определенный период
        /// </summary>
        /// <param name="idWorkStation"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<ActiveAlarm>> GetScopeActiveAlarm(int idWorkStation, DateTime startDate, DateTime endDate)
        {
            var result = await _httpClient.Get<List<ActiveAlarm>>($"/api/ActiveAlarm/ScopeByDatesAndIdWorkSt?idWorkStation={idWorkStation}&startDate={startDate:yyyy-MM-dd hh:mm:ss}&endDate={endDate:yyyy-MM-dd hh:mm:ss}") ?? new List<ActiveAlarm>();
            return result;
        }

        /// <summary>
        /// Возвращает все записи по Id рабочей станции за определенный период
        /// </summary>
        /// <param name="idWorkStation"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<SuppressedAlarm>> GetScopeSuppressedAlarm(int idWorkStation, DateTime startDate, DateTime endDate)
        {
            var result = await _httpClient.Get<List<SuppressedAlarm>>($"/api/SuppressedAlarm/ScopeByDatesAndIdWorkSt?idWorkStation={idWorkStation}&startDate={startDate:yyyy-MM-dd hh:mm:ss}&endDate={endDate:yyyy-MM-dd hh:mm:ss}") ?? new List<SuppressedAlarm>();
            return result;
        }
    }
}
