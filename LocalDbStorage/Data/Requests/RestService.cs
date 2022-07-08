using GPNA.ACSAPI.Repositories.Models;
using GPNA.AlarmControlSystem.LocalDbStorage.Data.Interfaces;


namespace GPNA.AlarmControlSystem.LocalDbStorage.Data.Requests
{

    public class RestService : IRestService
    {
        private readonly IRestClient _restClient;


        public RestService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        /// <summary>
        /// Получить все Дочерние общества
        /// </summary>
        /// <returns></returns>
        public async Task<Branch[]> GetAllBranch()
        {
            var result = await _restClient.GetAll<Branch>("/api/Branch?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Получить все Рабочие станции
        /// </summary>
        /// <returns></returns>
        public async Task<WorkStation[]> GetAllWorkStation()
        {
            var result = await _restClient.GetAll<WorkStation>("/api/WorkStation?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Получить все Активные аварии
        /// </summary>
        /// <returns></returns>
        public async Task<ActiveAlarm[]> GetAllActiveAlarm()
        {
            var result = await _restClient.GetAll<ActiveAlarm>("/api/ActiveAlarm?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Получить все Входящие аварии
        /// </summary>
        /// <returns></returns>
        public async Task<IncomingAlarm[]> GetAllIncomingAlarm()
        {
            var result = await _restClient.GetAll<IncomingAlarm>("/api/IncomingAlarm?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Получить все Подавленные аварии
        /// </summary>
        /// <returns></returns>
        public async Task<SuppressedAlarm[]> GetAllSuppressedAlarm()
        {
            var result = await _restClient.GetAll<SuppressedAlarm>("/api/SuppressedAlarm?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Получить все Теги
        /// </summary>
        /// <returns></returns>
        public async Task<Tag[]> GetAllTag()
        {
            var result = await _restClient.GetAll<Tag>("/api/Tag?itemsOnPage=0&page=1");
            return result;
        }

        /// <summary>
        /// Добавить тег
        /// </summary>
        /// <returns></returns>
        public async Task AddTag(Tag tag)
        {
            await _restClient.Post("/api/Tag", tag);
        }

        /// <summary>
        /// Изменить тег
        /// </summary>
        /// <returns></returns>
        public async Task UpdateTag(Tag tag, int id)
        {
            await _restClient.Put($"/api/Tag/{id}", tag);
        }

        /// <summary>
        /// Удалить тег
        /// </summary>
        /// <returns></returns>
        public async Task DeleteTag(int id)
        {
            await _restClient.Delete($"/api/Tag/{id}");
        }

    }
}




