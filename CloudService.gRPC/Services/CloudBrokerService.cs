using CommonModule.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CloudService.gRPC.Services
{
    public class CloudBrokerService : CloudBroker.CloudBrokerBase
    {
        private readonly ILogger<CloudBrokerService> _logger;
        public CloudBrokerService(ILogger<CloudBrokerService> logger)
        {
            _logger = logger;
        }

        public override Task<CloudResponse> Send(EquipmentEnrichedMessage request, ServerCallContext context)
        {
            return Task.FromResult(new CloudResponse
            {
                ReceivedTime = DateTime.UtcNow.ToTimestamp()
            });
        }
    }
}