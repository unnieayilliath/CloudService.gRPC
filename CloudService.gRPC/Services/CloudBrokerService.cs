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
    }
}