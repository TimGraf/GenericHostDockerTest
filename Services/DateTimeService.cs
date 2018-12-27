using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace GenericHostDockerTest.Services
{
    public class DateTimeService : IGenericService
    {
        private readonly ILogger _logger;
        private readonly string _serviceUrl;

        public DateTimeService(ILogger<DateTimeService> logger, IConfiguration config)
        {
            _logger = logger;

            var serviceConfig = config.GetSection("DateTimeService");

            _serviceUrl = serviceConfig["ServiceUrl"];
            _logger.LogInformation($"Date Time Service configured with URL: {_serviceUrl}.");
        }

        public void DoWork()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            var dateTimeData = client.GetStringAsync(_serviceUrl).GetAwaiter().GetResult();

            _logger.LogInformation($"Date Time Data: {dateTimeData}.");
        }
    }
}