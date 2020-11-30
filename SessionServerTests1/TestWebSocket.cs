using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SessionServerTests {

    public class TestWebSocket: IDisposable {
        public string Scheme { get; set; } = "ws";

        public string Path = "";

        public int ReceiveBufferSize { get; set; } = 1024;
        public int SendBufferSize { get; set; } = 1024;

        /// <summary>
        /// milis
        /// </summary>
        public int ReceiveTimeout { get; set; } = 50000;

        public TestServer Server { get; private set; }

        public WebSocketClient Client { get; private set; }

        public WebSocket Socket { get; private set; }

        ArraySegment<byte> buffer;

        public WebSocketReceiveResult LastResult { get; private set; } = null;

        public async Task Connect<StartUpClz>() where StartUpClz : class {
            var builder = WebHost.CreateDefaultBuilder()
                .UseStartup<StartUpClz>()
                .UseEnvironment("Development");

            Server = new TestServer(builder);
            Client = Server.CreateWebSocketClient();

            var uri = new UriBuilder(Server.BaseAddress) {
                Scheme = Scheme,
                Path = Path     
            };

            Socket = await Client.ConnectAsync(uri.Uri, CancellationToken.None);
            if (Socket == null) {
                throw new Exception("connect fail");
            }
            buffer = WebSocket.CreateClientBuffer(1024, 1024);
        }

        private void checkClose() {
            if (LastResult?.CloseStatus != null) {
                throw new Exception($"socket already closed / {LastResult?.CloseStatusDescription}");
            }
            if (Socket.State != WebSocketState.Open) {
                throw new Exception($"socket state !open / {Socket.State}");
            }
        }

        private CancellationToken newTimeoutToken() {
            var source = new CancellationTokenSource();
            source.CancelAfter(ReceiveTimeout);
            return source.Token;
        }

        /// <summary>
        /// websocket 으로부터 string을 하나 받습니다
        /// </summary>
        /// <returns>string 응답, throw 메시지 타입이 text가 아님</returns>
        public async Task<string> ReceiveString() {
            checkClose();

            var cancel = newTimeoutToken();

            var stringBuilder = new StringBuilder();
            while (true) {
                var res = await Socket.ReceiveAsync(buffer, cancel);
                LastResult = res;
                if (res.MessageType != WebSocketMessageType.Text) {
                    throw new Exception($"response message is not text / {res.MessageType}");
                }
                string str = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, res.Count);
                stringBuilder.Append(str);
                if (res.EndOfMessage) {
                    break;
                }
            }

            return stringBuilder.ToString();
        }

        public Task SendString(string s) {
            checkClose();
            var cancel = newTimeoutToken();
            var m = Encoding.UTF8.GetBytes(s);
           return Socket.SendAsync(m, WebSocketMessageType.Text, true, cancel);
        }

        public void Dispose() {
            Socket.Dispose();
            Server.Dispose();
        }
    }
}
