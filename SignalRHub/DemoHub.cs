using Microsoft.AspNetCore.SignalR;
namespace dotnet_mvc.SignalRHub
{
    public class DemoHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}