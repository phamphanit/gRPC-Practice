using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MeterReaderWeb.Data;
using MeterReaderWeb.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReaderWeb.Services
{
    public class MeterService : MeterReadingService.MeterReadingServiceBase
    {
        private readonly ILogger<MeterService> _logger;
        private readonly IReadingRepository _repository;
        public MeterService(ILogger<MeterService> logger,IReadingRepository repository)
        {
            this._logger = logger;
            this._repository = repository;
        }
        public override async Task<Empty> SendDiagnostics(IAsyncStreamReader<ReadingMessage> requestStream, ServerCallContext context)
        {
            var theTask = Task.Run(async () =>
            {
                await foreach (var reading in requestStream.ReadAllAsync())
                {
                    _logger.LogInformation($"Recieved Reading: {reading}");
                }
            });
            await theTask;
            return new Empty();
        }
        public async override Task<StatusMessage> AddReading(ReadingPacket request, ServerCallContext context)
        {
            var result = new StatusMessage()
            {
                Success = ReadingStatus.Failure
            };
            if(request.Successful == ReadingStatus.Success)
            {
                try
                {
                    foreach (var r in request.Readings)
                    {
                        if(r.ReadingValue < 1000)
                        {
                            _logger.LogDebug("Reading Value below acceptable level");
                            var trailer = new Metadata()
                            {
                                {"BadValue" , r.ReadingValue.ToString() },
                                {"Field" , "ReadingValue" },
                                {"BadValue" , "Readings are invalid" },
                            };
                            throw new RpcException(new Status(StatusCode.OutOfRange, "Value too low"),trailer);
                        }
                        //save to database
                        var reading = new MeterReading()
                        {
                            Value = r.ReadingValue,
                            ReadingDate = r.ReadingTime.ToDateTime(),
                            CustomerId = r.CustomerId
                        };
                        _repository.AddEntity(reading);
                    }
                    if(await _repository.SaveAllAsync())
                    {
                        result.Success = ReadingStatus.Success;
                        _logger.LogInformation($"Successful stored reading {request.Readings.Count}");
                    }
                }
                catch(RpcException)
                {
                    throw;
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Exeption throw during saving of readings: {ex}");

                    throw new RpcException(Status.DefaultCancelled,"Exception throw during process");
                }
               
            }
            return result;
        }
    }
}
