using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using WeatherForecastEtain.Core.Interfaces;
using WeatherForecastEtain.MVC.Models;

namespace WeatherForecastEtain.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IWeatherForcastService weatherForcastService;

        public HomeController(ILogger<HomeController> logger, IWeatherForcastService weatherForcastService)
        {
            this.logger = logger;
            this.weatherForcastService = weatherForcastService;
        }

        public async Task<IActionResult> Index()
        {
            logger.LogInformation("Index page loaded");

            return await Task.Run(() => View());
        }

        [Authorize]
        public async Task<IActionResult> Weather()
        {
            var weatherResults = await weatherForcastService.GetNextFiveDayForecast("Belfast");
            
            logger.LogInformation("Weather page loaded");
            return await Task.Run(() => View(weatherResults.Forecasts));
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
