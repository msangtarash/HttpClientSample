using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
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

    class Program
    {
        static async Task Main(string[] args)
        {
            // HttpClient HttpClient = new HttpClient(new HttpClientHandler());
            // HttpClient HttpClient = new HttpClient(new WebRequestHandler());
            // HttpClient HttpClient = new HttpClient(new WinHttpHandler());
            // AndroidHttpHandler | BrowserHttpHandler | NSUrlHttpHandler

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
        }
    }
}
