using log4net;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WGMansion.Bot.Settings;

namespace WGMansion.Bot.Services
{
    public interface IApiService
    {
        public Task<T> Get<T>(string endpoint, string query, string bearer);
        public Task<T> Post<T>(string endpoint, T query, string bearer);
    }

    public class ApiService : IApiService
    {
        private ILog _logger = LogManager.GetLogger(typeof(ApiService));
        private readonly ApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiService(IOptions<ApiSettings> apiSettings, IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> Get<T>(string endpoint, string query, string bearer)
        {
            try
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    var serializeOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    if (!string.IsNullOrEmpty(bearer))
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearer}");
                    var result = await client.GetAsync($"{_apiSettings.Url}/{endpoint}{query}");
                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStreamAsync();
                        return JsonSerializer.Deserialize<T>(content, serializeOptions);
                    }
                    throw new Exception(result.StatusCode.ToString());
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                throw;
            }
        }

        public async Task<T> Post<T>(string endpoint, T query, string bearer)
        {
            try
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    var serializeOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var json = JsonSerializer.Serialize(query, serializeOptions);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Clear();
                    if (!string.IsNullOrEmpty(bearer))
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearer}");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Accept", "*/*");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    var siteUri = new Uri(_apiSettings.Url);
                    client.BaseAddress = siteUri;

                    var fullUrl = $"{_apiSettings.Url}/{endpoint}";
                    var result = await client.PostAsync(fullUrl, content);
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new Exception($"{result.StatusCode}: {result.RequestMessage}");
                    }
                    var resultContent = await result.Content.ReadAsStreamAsync();
                    return JsonSerializer.Deserialize<T>(resultContent, serializeOptions);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                throw;
            }
        }
    }
}
