using Azure.Core;
using CloudService.EventHub.Interfaces;
using CloudService.EventHub.Services;
using CloudService.gRPC.Models;
using CommonModule.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Newtonsoft.Json;

namespace CloudService.gRPC.Services
{
    public class CloudBrokerService : CloudBroker.CloudBrokerBase
    {
        private readonly ILogger<CloudBrokerService> _logger;
        private readonly IEventHubService _eventHubSvc;
        public CloudBrokerService(ILogger<CloudBrokerService> logger)
        {
            _logger = logger;
            _eventHubSvc = new EventHubService();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<CloudResponse> Send(EquipmentEnrichedMessage request, ServerCallContext context)
        {
            var message = new EventHubMessage(request);

            //serialize it again to be send to event hub
            string serializedData = JsonConvert.SerializeObject(message);
            _logger.LogInformation(serializedData);
            // create event in azure event hub
            _eventHubSvc.CreateEventAsync(serializedData);
            return Task.FromResult(new CloudResponse
            {
                ReceivedTime = DateTime.UtcNow.ToTimestamp()
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CloudResponse> SendStream(IAsyncStreamReader<EquipmentEnrichedMessage> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {

                var message = new EventHubMessage(requestStream.Current);
                //serialize it again to be send to event hub
                string serializedData = JsonConvert.SerializeObject(message);
                _logger.LogInformation(serializedData);
                // create event in azure event hub
                _eventHubSvc.CreateEventAsync(serializedData);
            }
            Timestamp receivedTime = DateTime.UtcNow.ToTimestamp();
            return new CloudResponse
            {
                ReceivedTime = receivedTime
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task SendBiDirectionalStream(IAsyncStreamReader<EquipmentEnrichedMessage> requestStream, IServerStreamWriter<CloudResponse> responseStream, ServerCallContext context)
        {
            await foreach (var requestMessage in requestStream.ReadAllAsync(context.CancellationToken))
            {
                Timestamp receivedTime = DateTime.UtcNow.ToTimestamp();
                var message = new EventHubMessage(requestMessage);
                //serialize it again to be send to event hub
                string serializedData = JsonConvert.SerializeObject(message);
                _logger.LogInformation(serializedData);
                // create event in azure event hub
                _eventHubSvc.CreateEventAsync(serializedData);
                var response = new CloudResponse
                {
                    MessageId = requestMessage.MessageId,
                    ReceivedTime = receivedTime
                };
                await responseStream.WriteAsync(response);
            }
        }
    }
}