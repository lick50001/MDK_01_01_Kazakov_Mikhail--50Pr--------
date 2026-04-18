using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KeyPass.Models;
using Newtonsoft.Json;

namespace KeyPass.Context
{
    public class UserContext
    {
        static string url = "https://localhost:7105/user/";

        public static async Task<string> Login(string login, string password)
        {
            using (HttpClient Client = new HttpClient())
            {
                using (HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, url + "login"))
                {
                    Dictionary<string, string> FormData = new Dictionary<string, string>
                    {
                        ["login"] = login,
                        ["password"] = password
                    };

                    FormUrlEncodedContent Content = new FormUrlEncodedContent(FormData);
                    Request.Content = Content;
                    var Response = await Client.SendAsync(Request);
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        string sResponse = await Response.Content.ReadAsStringAsync();
                        Auth DataAuth = JsonConvert.DeserializeObject<Auth>(sResponse);
                        return DataAuth.Token;
                    } 
                }
            }
            return null;
        }

        public static async Task<bool> Register(string login, string password)
        {
            using (HttpClient Client = new HttpClient())
            {
                try
                {
                    Dictionary<string, string> FormData = new Dictionary<string, string>
                    {
                        ["login"] = login,
                        ["password"] = password
                    };

                    FormUrlEncodedContent Content = new FormUrlEncodedContent(FormData);

                    var Response = await Client.PostAsync(url + "register", Content);

                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else if (Response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        string errorMsg = await Response.Content.ReadAsStringAsync();
                        MessageBox.Show(errorMsg, "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка сервера при регистрации: {Response.StatusCode}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Не удалось подключиться к серверу: {e.Message}", "Ошибка сети", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
    }
}
