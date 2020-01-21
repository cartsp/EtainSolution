using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Net.Http;
using WeatherForecastEtain.Core.Interfaces;
using WeatherForecastEtain.Infrastructure.Services;

namespace WeatherForecastEtain
{
    public static class Startup
    {
        public static IHostBuilder GetHost(bool IsService)
        {
            return new HostBuilder()
                 .ConfigureAppConfiguration((hostContext, configApp) =>
                 {
                     configApp.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                 })
                 .ConfigureServices((hostContext, services) =>
                 {
                     var CurrentDir = Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar;
                     var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5));

                     services.AddLogging(configure => configure.AddSerilog(new LoggerConfiguration()
                               .MinimumLevel.Information()
                               .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                               .WriteTo.Async(a => a.File(CurrentDir + hostContext.Configuration
                                     .GetSection("AppSettings:LogLocation").Value))
                               .CreateLogger()))
                     .AddSingleton(hostContext.Configuration)
                     .AddHostedService<ConsoleApp>()
                     .AddSingleton<App>()
                     .AddHttpClient<IWeatherForcastService, MetaWeatherForcastService>()
                     .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[] //adding transient fault resilience
                     {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(4)
                     })).AddPolicyHandler(timeoutPolicy);  
                 });
        }
    }
}
