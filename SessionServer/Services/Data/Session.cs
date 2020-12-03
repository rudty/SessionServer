using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SessionServer.Services.Data {
    public class Session: IAsyncDisposable {
        private const int receiveTimeoutMills = 600 * 1000; // 10분

        readonly HttpContext context;

        readonly WebSocket webSocket;

        public string Id { get; private set; }

        public DateTime LoginTime { get; private set;} = DateTime.Now;

        public DateTime LastReceiveTime { get; private set; }
        
        public SessionStatus SessionStatus { get; private set; } = SessionStatus.Connect;

        public Session(HttpContext context, WebSocket sock) {
            this.context = context;
            this.webSocket = sock;
            this.Id = context.TraceIdentifier;
        }

        public Task SendText(string txt) {
            var m = Encoding.UTF8.GetBytes(txt);
            return webSocket.SendAsync(m, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private static CancellationToken newTimeoutToken() {
            var source = new CancellationTokenSource();
            source.CancelAfter(receiveTimeoutMills);
            return source.Token;
        }

        /// <summary>
        /// 클라이언트에서 뭔가를 받는 함수
        /// 실제로 처리하는건 아무것도 없고
        /// 마지막으로 응답 받은 시간만 갱신함
        /// </summary>
        /// <returns></returns>
        public async Task Run() {
            await SendText("serverhello");

            SessionStatus = SessionStatus.Ready;

            byte[] mem = new byte[1024];
            while (true) {
                ArraySegment<byte> buffer = new ArraySegment<byte>(mem);
                CancellationToken timeoutToken = newTimeoutToken();
                WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(buffer, timeoutToken);
                Console.WriteLine(Encoding.UTF8.GetString(mem, 0, receiveResult.Count));

                LastReceiveTime = DateTime.Now;

                if (receiveResult.CloseStatus != null) {
                    break;
                }
                if (SessionStatus != SessionStatus.Ready) {
                    break;
                }
            }
        }

        /// <summary>
        /// 커넥션을 종료합니다.
        /// 이 함수는 flag 만 변경하고
        /// 실제 종료는 
        /// 1. Run에서 break
        /// 2. DisposeAsync 
        /// </summary>
        public void Close() {
            SessionStatus = SessionStatus.Closed;
        }

        public async ValueTask DisposeAsync() {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, newTimeoutToken());
            webSocket.Dispose();
            return;
        }
    }
}
