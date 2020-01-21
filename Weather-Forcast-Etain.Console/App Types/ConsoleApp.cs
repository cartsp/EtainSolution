using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace WeatherForecastEtain
{
    public class ConsoleApp : IHostedService, IDisposable
    {
        #region Constructor and Properties

        private readonly App app;

        public ConsoleApp(App app)
        {
            this.app = app;
        }

        #endregion Constructor and Properties

        //App Start
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await app.StartAsync();
        }

        #region App Disposal
        public Task StopAsync(CancellationToken cancellationToken)
        {
             return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        #endregion App Disposal
    }
}
