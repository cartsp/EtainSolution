using System.Threading.Tasks;
using WeatherForecastEtain.Core.Entities;

namespace WeatherForecastEtain.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAndPasswordAsync(string username, string password);
    }
}
