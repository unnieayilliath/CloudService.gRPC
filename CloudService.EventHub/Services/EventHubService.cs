using Azure.Messaging.EventHubs.Producer;
using CloudService.EventHub.Interfaces;
using System.Text;
using Azure.Messaging.EventHubs;

namespace CloudService.EventHub.Services
{
    public class EventHubService : IEventHubService
    {
        private static readonly string _connectionString = "Endpoint=sb://cloudbroker-eventhub.servicebus.windows.net/;SharedAccessKeyName=createevents;SharedAccessKey=kUJRj4cydykjeRqYa5nJgWipHGzrkrIMp+AEhNuCJMs=;EntityPath=iot-sites";
        private static readonly string _eventHubName = "iot-sites";
        private static EventDataBatch? _eventBatch;
        private EventHubProducerClient _eventProducerClient;
        /// <summary>
        /// 
        /// </summary>
        public EventHubService()
        {
            _eventProducerClient = new EventHubProducerClient(_connectionString, _eventHubName);
        }
        private async Task<bool> CreateEventBatchAsync()
        {
            _eventBatch = await _eventProducerClient.CreateBatchAsync();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> CreateEventAsync(string data)
        {
            if (_eventBatch == null)
            {
                await CreateEventBatchAsync();
            }
            if (!_eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(data))))
            {
                // if it is too large for the batch
                throw new Exception($"Event is too large for the batch and cannot be sent.");
            }
            this.PublishEventsAsync();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> PublishEventsAsync()
        {
            await _eventProducerClient.SendAsync(_eventBatch);
            return true;
        }
    }
}
