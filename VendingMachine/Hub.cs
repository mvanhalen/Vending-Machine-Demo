using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Logic;
namespace VendingMachine
{
    public class VendingHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage", Program.VendingMachine);
        }

        public async Task ReceivedCoin(int cents)
        {
            Program.VendingMachine.CoinInsert(new Coin { Cents = cents, Quantity = 1, TotalCents = cents });
            await SendMessage();
           
        }
        public async Task ReceivedSale(int id)
        {
            await SendMessage();
            //await Clients.All.SendAsync("ReceiveMessage", Program.VendingMachine);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            return Clients.Groups(groups).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await SendMessage();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
