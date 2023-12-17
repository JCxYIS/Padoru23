using System.Net.WebSockets;
using System.Text;

namespace Padoru23Server.Services
{
    public class PadoruService
    {
        int _padorus = 0;
        List<WebSocket> _sockets = new List<WebSocket>();
        
        public void RegisterSocket(WebSocket socket)
        {
            _sockets.Add(socket);
            _sockets.RemoveAll(s => s.State != WebSocketState.Open);
        }

        public void SendPadoru()
        {
            _padorus++;
            var payload = Encoding.UTF8.GetBytes($"{_padorus}");
            Parallel.ForEach(_sockets, async socket =>
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(payload, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            });
        }

    }
}
