using dotnet_mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
namespace dotnet_mvc.SignalRHub
{
    public class AdminHub : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userManager.GetUserAsync(Context.User!);

            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "admin");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = await _userManager.GetUserAsync(Context.User!);

            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin");
            }

            await base.OnDisconnectedAsync(exception);
        }
        // public async Task SendMessage(string user, string message)
        // {
        //     await Clients.All.SendAsync("ReceiveMessage", user, message);
        // }
    }
}
