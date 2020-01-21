using System;

namespace WeatherForecastEtain.Core.Exceptions
{
    public class WeatherAPICommsException : Exception
    {
        public WeatherAPICommsException(string message) : base(message)
        {
        }

        public WeatherAPICommsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
