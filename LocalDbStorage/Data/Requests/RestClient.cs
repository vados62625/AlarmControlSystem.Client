using GPNA.AlarmControlSystem.LocalDbStorage.Data.Interfaces;
using LocalDbStorage.Data.Extentions;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GPNA.AlarmControlSystem.LocalDbStorage.Data.Requests
{
    public class RestClient : IRestClient
    {
        private readonly IHttpClientFactory _clientFactory;

        public RestClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T[]> GetAll<T>(string uri)
        {
            try
            {
                var client = _clientFactory.CreateClient(nameof(RestClient));
                var value = await client.GetAll<T>(uri);
                return value;
            }
            catch (Exception e)
            {

            }
            return new T[0];
        }

        public async Task<T?> GetOne<T>(string uri)
        {
            try
            {
                var client = _clientFactory.CreateClient(nameof(RestClient));
                var value = await client.GetOne<T>(uri);
                return value;
            }
            catch (Exception e)
            {

            }
            return default;
        }

        public async Task Post<T>(string uri, T identificator)
        {
            try
            {
                var client = _clientFactory.CreateClient(nameof(RestClient));
                await client.Post(uri, identificator);
            }
            catch (Exception e)
            {

            }
        }
    }
}
