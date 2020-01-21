using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WeatherForecastEtain.Core.Entities;
using WeatherForecastEtain.Core.Exceptions;
using WeatherForecastEtain.Core.Interfaces;

namespace WeatherForecastEtain.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<AccountService> logger;

        public AccountService(IUserRepository userRepository, ILogger<AccountService> logger)
        {
            this.logger = logger;
            this.userRepository = userRepository;
        }

        public async Task<User> Login(string username, string password)
        {
            try
            {
                return await userRepository.GetUserByUsernameAndPasswordAsync(username, password);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());

                throw new AccountLoginException(ex.ToString());
            }
        }
    }
}
