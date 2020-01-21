using System.Threading.Tasks;
using WeatherForecastEtain.Core.Entities;
using WeatherForecastEtain.Core.Interfaces;

namespace WeatherForecastEtain.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;

        public AccountService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<User> Login(string username, string password)
        {
            return await userRepository.GetUserByUsernameAndPasswordAsync(username, password);
        }
    }
}
