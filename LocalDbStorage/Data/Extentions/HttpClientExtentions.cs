﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;

namespace LocalDbStorage.Data.Extentions
{

    public static class HttpClientExtentions
    {
        static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        public static async Task<T[]> GetAll<T>(this HttpClient client, string uri)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = await client.GetAsync(uri);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //TODO Log trace
                    var buff = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<T[]>(buff, serializerOptions);

                    if (result != null)
                        return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return new T[0];
        }


        public static async Task<T?> GetOne<T>(this HttpClient client, string uri)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.GetAsync(uri);          
            if (response.StatusCode == HttpStatusCode.OK)
            {
           //TODO Log trace
                var buff = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(buff, serializerOptions); 
            }
            //TODO Log error
            return default;
        }

        public static async Task Post<T>(this HttpClient client, string uri, T identificator)
        {
            var jsonSer = JsonSerializer.Serialize(identificator);
            var httpContent = new StringContent(jsonSer, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Clear();

            await client.PostAsync(uri, httpContent);
        }

        public static async Task Put<T>(this HttpClient client, string uri, T identificator)
        {
            try
            {
                var jsonSer = JsonSerializer.Serialize(identificator);
                var httpContent = new StringContent(jsonSer, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Accept.Clear();

                var message = await client.PutAsync(uri, httpContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task Delete(this HttpClient client, string uri)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = await client.DeleteAsync(uri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}