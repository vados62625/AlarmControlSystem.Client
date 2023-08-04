using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace LocalDbStorage.Extentions
{

    public static class HttpClientExtentions
    {
        static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

        private static readonly Random Rnd = new();

        public static async Task<T> Get<T>(this HttpClient client, string uri, string requestId = "")
        {
            var httpreq = CreateRequest(HttpMethod.Get, uri, requestId);
            return await GetResponse<T>(client, httpreq);
        }
        
        public static async Task<T> Post<T>(this HttpClient client, string uri, object content, string requestId = "")
        {
            var httpreq = CreateRequest(HttpMethod.Post, uri, requestId);
            var json = JsonSerializer.Serialize(content);
            httpreq.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return await GetResponse<T>(client, httpreq);
        }

        public static async Task<T> Post<T>(this HttpClient client, string uri, string requestId = "")
        {
            var httpreq = CreateRequest(HttpMethod.Post, uri, requestId);
            return await GetResponse<T>(client, httpreq);
        }

        public static async Task<T> Put<T>(this HttpClient client, string uri, object content, string requestId = "")
        {
            var httpreq = CreateRequest(HttpMethod.Put, uri, requestId);
            var json = JsonSerializer.Serialize(content);
            httpreq.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return await GetResponse<T>(client, httpreq);
        }

        public static async Task<T> Delete<T>(this HttpClient client, string uri, string requestId = "")
        {
            var httpreq = CreateRequest(HttpMethod.Delete, uri, requestId);
            return await GetResponse<T>(client, httpreq);
        }

        public static HttpRequestMessage CreateRequest(HttpMethod method, string uri, string? requestId)
        {
            var httpreq = new HttpRequestMessage(method, uri);
            FillHeaders(httpreq.Headers, requestId);
            return httpreq;
        }

        public static void FillHeaders(HttpHeaders headers, string? requestId)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                requestId = GenerateRequestId();
            }

            headers.Add("Request-id", requestId);
        }

        public static string GenerateRequestId()
        {
            var rndBuff = new byte[8];
            Rnd.NextBytes(rndBuff);
            return Convert.ToBase64String(rndBuff);
        }

        public static async Task<T> GetResponse<T>(this HttpClient client, HttpRequestMessage request)
        {
            using var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                //TODO
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return default;
            }

            var content = await response.Content.ReadFromJsonAsync<T>();

            return GuardNullContent(content);
        }

        private static T GuardNullContent<T>(T? content)
        {
            if (content == null)
                throw new Exception($"Content cannot convert to type {typeof(T).Name}");

            return content;
        }

    }
}
