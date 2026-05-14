using Microsoft.AspNetCore.SignalR;

namespace AquaSense.Backend.Hubs;

// Clients connect here to receive live sensor updates.
// The MqttWorker calls hubContext.Clients.All.SendAsync("SensorUpdate", reading).
public class SensorHub : Hub
{
}
