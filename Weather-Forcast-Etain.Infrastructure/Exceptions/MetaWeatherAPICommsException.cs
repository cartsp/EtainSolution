using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherForecastEtain.Infrastructure.Exceptions
{
    class MetaWeatherAPICommsException : Exception
    {
        protected MetaWeatherAPICommsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

        public MetaWeatherAPICommsException(string message) : base(message)
        {
        }

        public MetaWeatherAPICommsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
