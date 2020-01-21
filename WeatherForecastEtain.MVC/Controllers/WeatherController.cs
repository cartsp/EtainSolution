using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using WeatherForecastEtain.Core.Interfaces;
using WeatherForecastEtain.MVC.Models;

namespace WeatherForecastEtain.MVC.Controllers
{
    public class WeatherController : Controller
    {
        private readonly ILogger<WeatherController> logger;
        private readonly IWeatherForcastService weatherForcastService;

        public WeatherController(ILogger<WeatherController> logger, IWeatherForcastService weatherForcastService)
        {
            this.logger = logger;
            this.weatherForcastService = weatherForcastService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var weatherResults = await weatherForcastService.GetNextFiveDayForecast("Belfast");

            logger.LogInformation("Weather page loaded");
            return View(weatherResults.Forecasts);
        }

        [Authorize]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetWeather()
        {
            var weatherResults = await weatherForcastService.GetNextFiveDayForecast("Belfast");

            return Json(weatherResults.Forecasts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return await Task.Run(() =>
                View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier })
            );
        }
    }
}