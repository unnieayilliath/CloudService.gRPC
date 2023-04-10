namespace CloudService.EventHub.Interfaces
{
    public interface IEventHubService
    {
        public Task<bool> CreateEventAsync(string data);
        public Task<bool> PublishEventsAsync();
    }
}