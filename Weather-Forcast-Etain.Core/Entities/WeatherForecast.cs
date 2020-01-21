using System.Collections.Immutable;

namespace WeatherForecastEtain.Core.Entities
{
    public class WeatherForecast
    {
        public string Location { get; }
        public IImmutableList<ForecastReading> Forecasts { get; }

        public WeatherForecast(string Location, IImmutableList<ForecastReading> Forecasts)
        {
            this.Location = Location;
            this.Forecasts = Forecasts;
        }       
    }
}
