using Application.Services.Exception;
using Contract.Services.CachingProvider;
using Contract.Services.ExternalServices;
using Contract.Services.IdentityProvider.DTOs.User;
using Domain.constant;
using Domain.Entities.Public;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Utility;

namespace Application.Services.ExternalServices
{
    public class SecondApiService : ISecondApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ICachingService<LoginResult> _cachingService;
        private readonly IHttpClientFactory _httpClientFactory;

        public SecondApiService(HttpClient httpClient, ICachingService<LoginResult> cachingService, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
            _cachingService = cachingService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<LoginResult> Login()
        {
            if (_cachingService.Exists("token"))
            {
                var token = await _cachingService.GetAsync("token");
                return token;
            }

            else
            {
                var login = new Login
                {
                    UserName = "admin",
                    Password = "Abcde@12345",
                    RememberMe = true,
                    ReturnUrl = ""
                };
                var content = new StringContent(JsonSerializer.Serialize(login, JsonSerializerSetting.JsonSerializerOptions), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/Accounts/login", content);

                if (!response.IsSuccessStatusCode)
                    throw new ServiceException(Messages.ErrorInCallExternalApiService);

                var result = await response.Content.ReadAsStringAsync();
                var finalResult = JsonSerializer.Deserialize<LoginResult>(result, JsonSerializerSetting.JsonDeserializerOptions);

                if (string.IsNullOrEmpty(finalResult.Token))
                    throw new ServiceException($"{Messages.ErrorInCallExternalApiService} Response: {result}");

                await _cachingService.SetAsync("token", finalResult);

                return finalResult;
            }
        }

        public async Task<IEnumerable<WeatherForecast>> WeatherForecastPolly()
        {
            var token = await Login();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer",token.Token);

            var response = await _httpClient.GetAsync("https://localhost:44360/WeatherForecast/getweatherforecast");

            if (!response.IsSuccessStatusCode)
                throw new ServiceException(Messages.ErrorInCallExternalApiService);

            var result = await response.Content.ReadAsStringAsync();
            var finalResult = JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(result, JsonSerializerSetting.JsonDeserializerOptions);

            if (finalResult is null)
                throw new ServiceException($"{Messages.ErrorInCallExternalApiService} Response: {result}");

            return finalResult;
        }

        public async Task<IEnumerable<WeatherForecast>> WeatherForecastClientFactory()
        {
            var httpClient = _httpClientFactory.CreateClient("SecondApi");
          
            var response = await httpClient.GetAsync("WeatherForecast/getweatherforecast");

            if (!response.IsSuccessStatusCode)
                throw new ServiceException(Messages.ErrorInCallExternalApiService);

            var result = await response.Content.ReadAsStringAsync();
            var finalResult = JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(result, JsonSerializerSetting.JsonDeserializerOptions);

            if (finalResult is null)
                throw new ServiceException($"{Messages.ErrorInCallExternalApiService} Response: {result}");

            return finalResult;
        }
    }
}
