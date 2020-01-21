using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherForecastEtain.Core.Interfaces;

namespace WeatherForecastEtain
{
    public class App
    {
        #region Constructor and Properties

        private readonly IConfiguration config;
        private readonly ILogger<App> logger;
        private readonly IWeatherForcastService weatherForecastService;

        public App(ILoggerFactory loggerFactory, IConfiguration configurationRoot,
            IWeatherForcastService weatherForecast)
        {
            logger = loggerFactory.CreateLogger<App>();
            config = configurationRoot;
            this.weatherForecastService = weatherForecast;
            logger.LogInformation("App is starting.");
        }

        #endregion Constructor and Properties

        public async Task StartAsync()
        {
            logger.LogInformation("Example app log at: {0}", DateTime.Now.ToShortDateString());
            
            var result = await weatherForecastService.Get5DayForecast("Belfast");
        }
    }
}
