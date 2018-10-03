using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Logic;
namespace VendingMachine
{
    /// <summary>
    /// 
    /// </summary>
    public class VendingHub : Hub
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage", Program.VendingMachine);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cents"></param>
        /// <returns></returns>
        public async Task ReceivedCoin(int cents)
        {
            Program.VendingMachine.CoinInsertCustomer(new Coin { Cents = cents, Quantity = 1, TotalCents = cents });
            await SendMessage();
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ReceivedSale(int id)
        {
            Program.VendingMachine.MakeSale(id);

            await SendMessage();
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Restock()
        {
            Program.VendingMachine = Machine.StartMachine();

            await SendMessage();

        }



        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            return Clients.Groups(groups).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await SendMessage();//return content directy
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
