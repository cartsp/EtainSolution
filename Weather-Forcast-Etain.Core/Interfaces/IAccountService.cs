using System.Threading.Tasks;
using WeatherForecastEtain.Core.Entities;

namespace WeatherForecastEtain.Core.Interfaces
{
    public interface IAccountService
    {
        Task<User> Login(string username, string password);
    }
}
