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
    public class Session {
        private const int receiveTimeoutMills = 600 * 1000; // 10분

        readonly HttpContext context;

        readonly WebSocket webSocket;

        public string Id { get; private set; }

        public DateTime LoginTime { get; private set;} = DateTime.Now;

        public SessionStatus SessionStatus { get; set; } = SessionStatus.Connect;

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

        public async Task Run() {
            byte[] mem = new byte[1024];
            while (true) {
                if (SessionStatus != SessionStatus.Ready) {
                    break;
                }
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
