using dotnet_mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace dotnet_mvc.SignalRHub
{
    public class NotificationHub : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            var userId = Context.UserIdentifier; 

            if (user != null && user.IsInRole("Admin")) 
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "admin");
            }
            else if (userId != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;

            if (userId != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin");

            await base.OnDisconnectedAsync(exception);
        }


        // public async Task SendMessage(string user, string message)
        // {
        //     await Clients.All.SendAsync("ReceiveMessage", user, message);
        // }


    }
}
