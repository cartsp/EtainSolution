using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostedServiceAsAService;

namespace WeatherForecastEtain
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var isService = true;

            isService = !(Debugger.IsAttached); // --use for service, enables console debugging
            try
            {
                if (isService)
                {
                    await Startup.GetHost(isService).RunAsServiceAsync();
                }
                else
                {
                    using (var host = Startup.GetHost(isService).Build())
                    {
                        await host.StartAsync();
                    }
                }
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
