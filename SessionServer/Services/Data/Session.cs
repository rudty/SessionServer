using Microsoft.AspNetCore.Http;
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
        [JsonIgnore]
        readonly HttpContext context;

        [JsonIgnore]
        readonly WebSocket webSocket;

        public string Id { get; private set; }

        public DateTime LoginTime { get; private set;} = DateTime.Now;

        public Session(HttpContext context, WebSocket sock) {
            this.context = context;
            this.webSocket = sock;
            this.Id = context.TraceIdentifier;
        }

        public Task SendText(string txt) {
            var m = Encoding.UTF8.GetBytes(txt);
            return webSocket.SendAsync(m, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
