using Challenge.web.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Challenge.web.Services
{
    public class BaseService
    {
        private readonly IHttpClientFactory httpClient;

        public BaseService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("ChallengeApi");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if(apiRequest.Headers != null)
                {
                    foreach (var head in apiRequest.Headers)
                    {
                        client.DefaultRequestHeaders.Add(head.Key, head.Value);
                    }
                }
                

                if (apiRequest.Data != null)
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");

                if (apiRequest.Authorize)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);

                HttpResponseMessage apiResponse = null;
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                
                if(apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(JsonConvert.DeserializeObject<string>(apiContent));
                }

                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDto;
            }
            catch (Exception error)
            {
                throw new Exception(String.IsNullOrEmpty(error.Message) ? "Ocurrio un error al realizar la consulta" : error.Message);
            }
        }
    }
}
