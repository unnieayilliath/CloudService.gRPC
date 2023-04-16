using CommonModule.Protos;
using Google.Protobuf.WellKnownTypes;

namespace CloudService.gRPC.Models
{
    public class EventHubMessage
    {

        public EventHubMessage(EquipmentEnrichedMessage message)
        {
            Id= Guid.NewGuid().ToString();
            DeviceId = message.DeviceId;
            Status = message.Status;
            Temperature =message.Temperature;
            Timestamp = message.Timestamp;
            RoomHumidity =message.RoomHumidity;
            RoomTemperature=message.RoomTemperature;
            Dutymanager = message.Dutymanager;
            EnergyConsumption = message.EnergyConsumption;
            FactoryId = message.FactoryId;
            Payload = message.Payload;
        }

        /// <summary>
        /// This property is the unique message id
        /// </summary>
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public Timestamp Timestamp { get; set; }
        public float RoomTemperature { get; set; }
        public float Temperature { get; set; }
        public float RoomHumidity { get; set; }
        public string Dutymanager { get; set; }
        public float EnergyConsumption { get; set; }
        public string FactoryId { get; set; }
        public string Payload { get; set; }
        public string Status { get; set; }
        

    }
}
