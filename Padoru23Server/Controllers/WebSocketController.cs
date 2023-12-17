using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Padoru23Server.Services;

namespace Padoru23Server.Controllers
{
    public class WebSocketController : ControllerBase
    {
        PadoruService _padoruService;
        ILogger<WebSocketController> _logger;
        public WebSocketController(PadoruService padoruService, ILogger<WebSocketController> logger)
        {
            _padoruService = padoruService;
            _logger = logger;
        }

        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                _logger.LogDebug("Hashinesoriyo");
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _padoruService.RegisterSocket(webSocket);
                await ListenPadoru(webSocket);                
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        [Route("/padoru")]
        public void Padoru() // send by http
        {
            _logger.LogDebug("Padorupadoru");
            _padoruService.SendPadoru();
        }

        private async Task ListenPadoru(WebSocket webSocket)
        {
            var buffer = new byte[64 * 4];
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                _padoruService.SendPadoru(); // send by socket
                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            
            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }


        //private static async Task Echo(WebSocket webSocket)
        //{
        //    var buffer = new byte[1024 * 4];
        //    var receiveResult = await webSocket.ReceiveAsync(
        //        new ArraySegment<byte>(buffer), CancellationToken.None);


        //    while (!receiveResult.CloseStatus.HasValue)
        //    {
        //        await webSocket.SendAsync(
        //            new ArraySegment<byte>(buffer, 0, receiveResult.Count),
        //            receiveResult.MessageType,
        //            receiveResult.EndOfMessage,
        //            CancellationToken.None);

        //        receiveResult = await webSocket.ReceiveAsync(
        //            new ArraySegment<byte>(buffer), CancellationToken.None);
        //    }

        //    await webSocket.CloseAsync(
        //        receiveResult.CloseStatus.Value,
        //        receiveResult.CloseStatusDescription,
        //        CancellationToken.None);
        //}
    }
}