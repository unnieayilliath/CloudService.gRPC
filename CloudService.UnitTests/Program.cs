// See https://aka.ms/new-console-template for more information
using CloudService.EventHub.Services;

var evntHubSvc = new EventHubService();
await evntHubSvc.CreateEventAsync("this is my data");
await evntHubSvc.CreateEventAsync("this is my data");
await evntHubSvc.CreateEventAsync("this is my data");
await evntHubSvc.CreateEventAsync("this is my data");
await evntHubSvc.PublishEventsAsync();
