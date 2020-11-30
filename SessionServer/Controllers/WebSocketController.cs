using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SessionServer.Services;
using SessionServer.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SessionServer.Controllers {
    [Route("api/[controller]")]
    public class WebSocketController : Controller {

        private const int receiveTimeoutMills = 600 * 1000; // 10분
        private readonly SessionService sessionService;

        public WebSocketController(SessionService svc) {
            this.sessionService = svc;
        }

        [HttpGet]
        public async Task Index() {
            HttpContext context = ControllerContext.HttpContext;
            WebSocketManager websocketManager = context.WebSockets;
            
            if (!websocketManager.IsWebSocketRequest) {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }
            
            WebSocket webSocket = await websocketManager.AcceptWebSocketAsync();
            Session session = sessionService.Register(context, webSocket);

            await session.SendText("serverhello");
            await receiveMessage(webSocket);
        }

        private static CancellationToken newTimeoutToken() {
            var source = new CancellationTokenSource();
            source.CancelAfter(receiveTimeoutMills);
            return source.Token;
        }


        /// <summary>
        /// 현재는 뭔가 받아도 할게 딱히 없지만 이후 추가되면 서비스 하나 더만들어서 사용할 것
        /// </summary>
        /// <param name="webSocket">입력 소켓</param>
        /// <returns></returns>
        private async Task receiveMessage(WebSocket webSocket) {
            byte [] mem = new byte[1024];
            while (true) {
                ArraySegment<byte> buffer = new ArraySegment<byte>(mem);
                CancellationToken timeoutToken = newTimeoutToken();
                WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(buffer, timeoutToken);
                Console.WriteLine(Encoding.UTF8.GetString(mem, 0, receiveResult.Count));

                if (receiveResult.CloseStatus != null) {
                    break;
                }
            }
        }
    }
}
