namespace WeatherForecastEtain.Core.Entities
{
    public class User
    {
        public string Username { get; }
        public string Roles { get; }
        public string Password { get; }

        public User(string Username, string Password, string Roles)
        {
            this.Username = Username;
            this.Roles = Roles;
            this.Password = Password;
        }
    }
}
