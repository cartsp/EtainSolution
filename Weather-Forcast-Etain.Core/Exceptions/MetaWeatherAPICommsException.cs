using System;

namespace WeatherForecastEtain.Core.Exceptions
{
    public class MetaWeatherAPICommsException : Exception
    {
        public MetaWeatherAPICommsException(string message) : base(message)
        {
        }

        public MetaWeatherAPICommsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
