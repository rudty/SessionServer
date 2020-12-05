using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SessionServer.AutoDependencyInjection;
using SessionServer.Services.Data;
using SessionServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SessionServer.Services {
    [Component]
    public class SocketMessageService {
        private const int receiveTimeoutMills = 600 * 1000; // 10분
        readonly ILogger<SocketMessageService> logger;

        public SocketMessageService(ILogger<SocketMessageService> logger) {
            this.logger = logger;
        }

        private static CancellationToken newTimeoutToken() {
            var source = new CancellationTokenSource();
            source.CancelAfter(receiveTimeoutMills);
            return source.Token;
        }

        private static Task CloseWebSocket(WebSocket webSocket, string reason) {
            return webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, reason, newTimeoutToken());
        }

        /// <summary>
        /// 클라이언트에서 뭔가를 받는 함수
        /// 실제로 처리하는건 아무것도 없고
        /// 마지막으로 응답 받은 시간만 갱신함
        /// </summary>
        /// <returns></returns>
        public async Task HandleMessage(Session session, WebSocket webSocket) {
            await session.SendText("serverhello");
            logger.I($"[{session.Id}] session hello {DateTime.Now} ");
            session.SessionStatus = SessionStatus.Ready;

            byte[] mem = new byte[1024];
            while (true) {
                ArraySegment<byte> buffer = new ArraySegment<byte>(mem);
                CancellationToken timeoutToken = newTimeoutToken();
                WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(buffer, timeoutToken);

                if (receiveResult.Count > 0) {
                    Console.WriteLine(Encoding.UTF8.GetString(mem, 0, receiveResult.Count));
                    session.LastReceiveTime = DateTime.Now;
                }

                if (receiveResult.CloseStatus != null) {
                    session.LastCloseDescription = receiveResult.CloseStatusDescription;
                    Console.WriteLine($"close: {receiveResult.CloseStatus} {receiveResult.CloseStatusDescription}");
                    break;
                }
                if (session.SessionStatus != SessionStatus.Ready) {
                    break;
                }
            }

            session.SessionStatus = SessionStatus.Closed;
            logger.I($"[{session.Id}] session bye {session.LastCloseDescription}\t{DateTime.Now} ");
            await CloseWebSocket(webSocket, session.LastCloseDescription);
        }
    }
}
