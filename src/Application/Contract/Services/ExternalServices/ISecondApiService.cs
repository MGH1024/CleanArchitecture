using Contract.Services.IdentityProvider.DTOs.User;
using Domain.Entities.Public;

namespace Contract.Services.ExternalServices
{
    public interface ISecondApiService
    {
        Task<LoginResult> Login();
        Task<IEnumerable<WeatherForecast>> WeatherForecastPolly();
        Task<IEnumerable<WeatherForecast>> WeatherForecastClientFactory();
    }
}
