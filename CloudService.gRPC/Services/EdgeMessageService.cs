using CloudService.gRPC;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CloudService.gRPC.Services
{
    public class EdgeMessageService : EdgeMessage.EdgeMessageBase
    {
        private readonly ILogger<EdgeMessageService> _logger;
        public EdgeMessageService(ILogger<EdgeMessageService> logger)
        {
            _logger = logger;
        }

        public override Task<CloudResponse> Send(CloudRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CloudResponse
            {
                ReceivedTime = DateTime.UtcNow.ToTimestamp()
            });
        }
    }
}