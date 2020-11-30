using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SessionServer.Services.Data {
    public class Session {
        public HttpContext Context{ get; init; }

        readonly WebSocket webSocket;

        public string Id { get; private set; }

        public Session(HttpContext context, WebSocket sock) {
            this.Context = context;
            this.webSocket = sock;
            this.Id = context.TraceIdentifier;
        }

        public Task SendText(string txt) {
            var m = Encoding.UTF8.GetBytes(txt);
            return webSocket.SendAsync(m, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
