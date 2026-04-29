using Core.Interfaces.SignalRInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    public class RobotHub : Hub<IRobotClient>
    {
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"Client connected: {connectionId}");
            await Clients.Client(connectionId)
                .ReceiveNotification("The medical robot server was successfully connected.");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"Client disconnected: {connectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMoveCommand(string direction)
        {
            // Send the command to all connected devices "except" the one that sent the command (the mobile phone)
            // This ensures that only the robot receives the character
            await Clients.Others.ReceiveMovementCommand(direction);
            Console.WriteLine($"Movement command sent: {direction}");
        }
    }
}
