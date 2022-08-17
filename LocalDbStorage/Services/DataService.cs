using LocalDbStorage.Extentions;
using LocalDbStorage.Interfaces;
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
        
        /// <summary>
        /// Получить все Дочерние общества
        /// </summary>
        /// <returns></returns>
        public async Task<List<Branch>> GetAllBranch()
        {
            var result = await _httpClient.Get<List<Branch>>("/api/Branch?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Получить все Рабочие станции
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkStation>> GetAllWorkStation()
        {
            var result = await _httpClient.Get<List<WorkStation>>("/api/WorkStation?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Получить все Активные аварии
        /// </summary>
        /// <returns></returns>
        public async Task<List<ActiveAlarm>> GetAllActiveAlarm()
        {
            var result = await _httpClient.Get<List<ActiveAlarm>>("/api/ActiveAlarm?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Получить все Входящие аварии
        /// </summary>
        /// <returns></returns>
        public async Task<List<IncomingAlarm>> GetAllIncomingAlarm()
        {
            var result = await _httpClient.Get<List<IncomingAlarm>>("/api/IncomingAlarm?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Получить все Подавленные аварии
        /// </summary>
        /// <returns></returns>
        public async Task<List<SuppressedAlarm>> GetAllSuppressedAlarm()
        {
            var result = await _httpClient.Get<List<SuppressedAlarm>>("/api/SuppressedAlarm?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Получить все Теги
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tag>> GetAllTag()
        {
            var result = await _httpClient.Get<List<Tag>>("/api/Tag?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Добавить тег
        /// </summary>
        /// <returns></returns>
        public async Task AddTag(Tag tag)
        {
            await _httpClient.Post<Tag>("/api/Tag", tag);
        }

        /// <summary>
        /// Изменить тег
        /// </summary>
        /// <returns></returns>
        public async Task UpdateTag(Tag tag, int id)
        {
            await _httpClient.Put<Tag>($"/api/Tag/{id}", tag);
        }

        /// <summary>
        /// Удалить тег
        /// </summary>
        /// <returns></returns>
        public async Task DeleteTag(Tag tag, int id)
        {
            await _httpClient.Delete<Tag>($"/api/Tag/{id}");
        }
    }
}
