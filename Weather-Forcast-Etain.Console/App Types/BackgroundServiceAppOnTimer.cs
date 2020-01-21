using Microsoft.Extensions.Hosting;
using WeatherForecastEtain;
using System;
using System.Threading;
using System.Threading.Tasks;

public class BackgroundServiceAppOnTimer : IHostedService, IDisposable
{
    #region Constructor and Properties

    private readonly App app;

    public BackgroundServiceAppOnTimer(App app)
    {
        this.app = app;
    }
    #endregion Constructor and Properties

    private Timer _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(
            async (e) => await App(),
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }

    public async Task App()
    {
        await app.StartAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}