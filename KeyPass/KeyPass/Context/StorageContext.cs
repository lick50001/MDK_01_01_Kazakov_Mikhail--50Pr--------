using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using KeyPass.Models;

namespace KeyPass.Context
{
    public class StorageContext
    {
        static string url = "https://localhost:7105/storage/";

        public static async Task<List<Storage>> Get()
        {
            using (HttpClient Client = new HttpClient())
            {
                using (HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Get, url + "get"))
                {
                    Request.Headers.Add("token", MainWindow.Token);
                    var Response = await Client.SendAsync(Request);
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        string sResponse = await Response.Content.ReadAsStringAsync();
                        List<Storage> Storages = JsonConvert.DeserializeObject<List<Storage>>(sResponse);
                        return Storages;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Добавление новой записи в хранилище
        /// POST /storage/add
        /// </summary>
        /// <param name="storage">Объект для добавления (без ID)</param>
        /// <returns>Созданный объект с присвоенным ID или null при ошибке</returns>
        public static async Task<Storage> Add(Storage storage)
        {
            using (HttpClient Client = new HttpClient())
            {
                using (HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, url + "add"))
                {
                    Request.Headers.Add("token", MainWindow.Token);
                    string JsonStorage = JsonConvert.SerializeObject(storage);
                    var Content = new StringContent(JsonStorage, Encoding.UTF8, "application/json");
                    Request.Content = Content;
                    var Response = await Client.SendAsync(Request);
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        string sResponse = await Response.Content.ReadAsStringAsync();
                        // Десериализуем ответ (сервер возвращает созданный объект с ID)
                        Storage Storage = JsonConvert.DeserializeObject<Storage>(sResponse);
                        return Storage;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Обновление существующей записи
        /// PUT /storage/update
        /// </summary>
        /// <param name="storage">Объект с обновленными данными (должен содержать ID)</param>
        /// <returns>Обновленный объект или null при ошибке</returns>
        public static async Task<Storage> Update(Storage storage)
        {
            using (HttpClient Client = new HttpClient())
            {
                using (HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Put, url + "update"))
                {
                    Request.Headers.Add("token", MainWindow.Token);
                    string JsonStorage = JsonConvert.SerializeObject(storage);
                    var Content = new StringContent(JsonStorage, Encoding.UTF8, "application/json");
                    Request.Content = Content;
                    var Response = await Client.SendAsync(Request);
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        string sResponse = await Response.Content.ReadAsStringAsync();
                        Storage Storage = JsonConvert.DeserializeObject<Storage>(sResponse);
                        return Storage;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Удаление записи по ID
        /// DELETE /storage/delete
        /// </summary>
        /// <param name="id">ID удаляемой записи</param>
        public static async Task Delete(int id)
        {
            using (HttpClient Client = new HttpClient())
            {
                using (HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Delete, url + "delete"))
                {
                    Request.Headers.Add("token", MainWindow.Token);
                    Dictionary<string, string> FormData = new Dictionary<string, string>
                    {
                        ["id"] = id.ToString()
                    };
                    FormUrlEncodedContent Content = new FormUrlEncodedContent(FormData);
                    Request.Content = Content;
                    var Response = await Client.SendAsync(Request);
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        string sResponse = await Response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
    }
}
