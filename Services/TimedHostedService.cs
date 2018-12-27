using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace GenericHostDockerTest.Services
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IDateTimeService _dateTimeService;
        private readonly Int16 _pollingInterval;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger, IConfiguration config, IDateTimeService dateTimeService)
        {
            _logger = logger;
            _dateTimeService = dateTimeService;

            var serviceConfig = config.GetSection("TimedService");

            _pollingInterval = Int16.Parse(serviceConfig["PollingIntervalSeconds"]);

            _logger.LogInformation($"Timed Service configured with polling interval: {_pollingInterval} seconds.");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_pollingInterval));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");

            var dateTimeData = _dateTimeService.GetDateTimeData();

            _logger.LogInformation($"Date Time Data: {dateTimeData}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}