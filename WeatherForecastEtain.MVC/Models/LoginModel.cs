using System.ComponentModel.DataAnnotations;

namespace WeatherForecastEtain.MVC.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
