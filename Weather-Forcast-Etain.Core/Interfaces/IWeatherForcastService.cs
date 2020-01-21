using System.Threading.Tasks;
using WeatherForecastEtain.Core.Entities;

namespace WeatherForecastEtain.Core.Interfaces
{
    public interface IWeatherForcastService
    {
        Task<WeatherForecast> GetNextFiveDayForecast(string location);
    }
}
