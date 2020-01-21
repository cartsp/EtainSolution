using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherForecastEtain.Core.Entities;
using WeatherForecastEtain.Core.Interfaces;
using WeatherForecastEtain.Core.Exceptions;
using WeatherForecastEtain.Infrastructure.DTOs.MetaWeatherAPI;

namespace WeatherForecastEtain.Infrastructure.Services
{
    public class MetaWeatherForcastService : IWeatherForcastService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<MetaWeatherForcastService> logger;

        public MetaWeatherForcastService(HttpClient httpClient, ILogger<MetaWeatherForcastService> logger)
        { 
            this.logger = logger;
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("https://www.metaweather.com/api/");
        }

        /// <summary>
        /// Returns a 5 day weather forecast from the MetaWeather api
        /// </summary>
        /// <param name="location">Location to check</param>
        /// <returns>The 5 day weather forecast for the specified location</returns>
        public async Task<WeatherForecast> GetNextFiveDayForecast(string location)
        {
            logger.LogInformation($"Beginning contact with MetaWeather API at: {DateTime.Now}");

            try
            {
                var metaWeatherLocationID = await GetMetaWeatherLocationID(location);

                var response = await httpClient.GetAsync($"location/{metaWeatherLocationID}/");

                response.EnsureSuccessStatusCode();

                var metaWeatherForecast = await JsonSerializer.DeserializeAsync<MetaWeatherForecastLocationSearchResponse>(await response.Content.ReadAsStreamAsync());

                logger.LogInformation($"Ending contact with MetaWeather API at: {DateTime.Now}");

                return new WeatherForecast(location, metaWeatherForecast.consolidated_weather
                                                    .OrderBy(weather => weather.applicable_date)
                                                    .Skip(1) //question asked for next 5 days of weather so skipping todays date
                                                    .Take(5)
                                                    .Select(weather =>
                                                        new ForecastReading(
                                                            TemperatureInC: weather.the_temp,
                                                            Date: DateTime.Parse(weather.applicable_date),
                                                            WeatherState: weather.weather_state_name,
                                                            WeatherStateAbbreviation: weather.weather_state_abbr))
                                                    .ToImmutableList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());

                throw new WeatherAPICommsException(location, ex);
            }
        }

        /// <summary>
        /// MetaWeather requires a location ID to retreive weather information
        /// </summary>
        /// <param name="location">The location to search</param>
        /// <returns>The MetaWeather WOEID based on location used search the API for forecasts</returns>
        private async Task<int> GetMetaWeatherLocationID(string location)
        {
            try
            {
                var response = await httpClient.GetAsync($"location/search/?query={location}");

                response.EnsureSuccessStatusCode();

                var metaWeatherForecast = await JsonSerializer.DeserializeAsync<List<LocationInfo>>(await response.Content.ReadAsStreamAsync());

                return metaWeatherForecast.FirstOrDefault().woeid;  //using first for demo, could do more here if needed
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());

                throw new WeatherAPICommsException($"location/search/?query={location}", ex);
            }
        }
    }
}
