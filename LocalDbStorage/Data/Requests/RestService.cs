using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    }
}




