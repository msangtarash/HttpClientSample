﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpSamples
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }

    public class UserData
    {
        [JsonProperty("data")]
        public User User { get; set; }
    }

    public interface IUsersService
    {
        [Get("/users/{userId}")]
        Task<UserData> GetUserById(int userId);

        [Post("/users")]
        Task<object> CreateUser(object user);
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            // HttpClient HttpClient = new HttpClient(new HttpClientHandler());
            // HttpClient HttpClient = new HttpClient(new WebRequestHandler());
            // HttpClient HttpClient = new HttpClient(new WinHttpHandler());
            // AndroidHttpHandler | BrowserHttpHandler | NSUrlHttpHandler

            { // get

                // http client
                HttpClient httpClient = new HttpClient();

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://reqres.in/api/users/2") { })
                {
                    using (HttpResponseMessage response = await httpClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        using (StreamReader streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                        {
                            using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                            {
                                JToken json = await JToken.LoadAsync(jsonReader);
                                User user = json["data"].ToObject<User>();
                            }
                        }
                    }
                }

                // refit
                User user2 = (await RestService.For<IUsersService>("https://reqres.in/api").GetUserById(2)).User;
            }

            { // post

                // http client
                HttpClient httpClient = new HttpClient();

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://reqres.in/api/users")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new { name = "morpheus", job = "leader" }), Encoding.UTF8, "text/json")
                })
                {
                    using (HttpResponseMessage response = await httpClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();

                        using (StreamReader streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                        {
                            using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                            {
                                JToken json = await JToken.LoadAsync(jsonReader);
                            }
                        }
                    }
                }

                // refit
                var createdUser = await RestService.For<IUsersService>("https://reqres.in/api").CreateUser(new { name = "morpheus", job = "leader" });
            }
        }
    }
}
