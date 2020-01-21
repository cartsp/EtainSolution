using System;

namespace WeatherForecastEtain.Core.Entities
{
    public class ForecastReading
    {
        public float TemperatureInC { get; }
        public DateTime Date { get; }
        public string WeatherState { get; }
        public string WeatherStateAbbreviation { get; }
        
        public ForecastReading(float TemperatureInC, DateTime Date, string WeatherState, string WeatherStateAbbreviation)
        {
            this.TemperatureInC = TemperatureInC;
            this.Date = Date;
            this.WeatherState = WeatherState;
            this.WeatherStateAbbreviation = WeatherStateAbbreviation;
        }
    }
}