using Application.Interfaces.SignalRInterfaces;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    //[Authorize]
    [AllowAnonymous]
    public class RobotHub : Hub<IRobotClient>
    {
        private readonly RobotConnectionTracker _tracker;

        public RobotHub(RobotConnectionTracker tracker)
        {
            _tracker = tracker;
        }
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"Client connected: {connectionId}");

            var deviceType = Context.GetHttpContext()?.Request.Query["deviceType"].ToString();
            if (deviceType == "robot")
            {
                _tracker.IsRobotConnected = true;
                Console.WriteLine("🤖 Medical Robot is now ONLINE!"); 
            }

            await Clients.Client(connectionId)
                .ReceiveNotification("The medical robot server was successfully connected.");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"Client disconnected: {connectionId}");

            var deviceType = Context.GetHttpContext()?.Request.Query["deviceType"].ToString();
            if (deviceType == "robot")
            {
                _tracker.IsRobotConnected = false;
                Console.WriteLine("⚠️ Medical Robot is OFFLINE!"); 
            }

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
