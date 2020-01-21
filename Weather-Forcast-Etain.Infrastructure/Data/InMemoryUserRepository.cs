using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastEtain.Core.Entities;
using WeatherForecastEtain.Core.Interfaces;

namespace WeatherForecastEtain.Infrastructure.Data
{
    public class InMemoryUserRepository : IUserRepository
    {
        //using this instead of a database for the demo, or I will never get any sleep! :)
        private static ImmutableList<User> users = ImmutableList.Create(
            new User("admin", "admin", "Admin,User" ),
            new User("user", "user", "User" )
        );

        public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            return await Task.FromResult(users.Where(u => u.Username.ToLower() == username.ToLower() 
                                 && u.Password.ToLower() == password.ToLower())
                                .FirstOrDefault());
        }
    }
}
