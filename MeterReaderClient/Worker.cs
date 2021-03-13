using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using MeterReaderWeb.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MeterReaderClient
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly ReadingFactory readingFactory;
        private MeterReadingService.MeterReadingServiceClient _client = null;
        public Worker(ILogger<Worker> logger,IConfiguration config,ReadingFactory readingFactory)
        {
            _logger = logger;
            this._config = config;
            this.readingFactory = readingFactory;
        }
        protected MeterReadingService.MeterReadingServiceClient Client
        {
            get
            {
                if(_client == null)
                {
                    var channel = GrpcChannel.ForAddress(_config["Service:ServerUrl"]);
                    _client = new MeterReadingService.MeterReadingServiceClient(channel);
                }
                return _client;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var customerId = _config.GetValue<int>("Service:CustomerId");
                var pkt = new ReadingPacket()
                {
                    Successful = ReadingStatus.Success,
                    Notes = "This is our test"
                };
               
                for(var x=0; x<2;x++)
                {
                    pkt.Readings.Add(await readingFactory.Generate(customerId));
                }

                var result = await Client.AddReadingAsync(pkt);
                if(result.Success == ReadingStatus.Success)
                {
                    _logger.LogInformation("Successful sent");
                }
                else
                {
                    _logger.LogInformation("Failed to send");
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(_config.GetValue<int>("Service:DelayInterval"), stoppingToken);
            }
        }
    }
}
