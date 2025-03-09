using Microsoft.AspNetCore.SignalR;

namespace ProcivisPensionDemo.Server.Services
{
    public class QrCodeHub : Hub
    {
        public async Task NotifyApproval(string qrResponse)
        {
            await Clients.All.SendAsync("QrCodeApproved", qrResponse);
        }
    }
}
