using System;

namespace WeatherForecastEtain.Core.Exceptions
{
    public class AccountLoginException : Exception
    {
        public AccountLoginException(string message) : base(message)
        {
        }

        public AccountLoginException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
