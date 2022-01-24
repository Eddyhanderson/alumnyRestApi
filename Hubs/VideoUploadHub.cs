using alumni.IServices;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Hubs
{
    public class VideoUploadHub : Hub<IVideoUploadHub>
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;           
        }

        public async Task SendVideoInfo(string videoInfo, string connectionId)
        {
            await Clients.Client(connectionId).ReceiveVideoInfo(videoInfo);
        }

        
    }

    public interface IVideoUploadHub
    {
        Task ReceiveVideoInfo(string videoInfo);
        Task ReceiveConnectionId(string connectionId);
    }    
}
