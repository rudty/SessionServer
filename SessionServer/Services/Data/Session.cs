using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
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

        readonly WebSocket webSocket;

        public string Id { get; private set; }

        public DateTime LoginTime { get; private set;} = DateTime.Now;

        public DateTime LastReceiveTime { get; internal set; }
        
        public SessionStatus SessionStatus { get; internal set; } = SessionStatus.Connect;

        /// <summary>
        /// 서버에서 종료시키거나 클라이언트가 종료 요청 보냈을때 저장
        /// </summary>
        public string LastCloseDescription { get; internal set; } = "";

        public Session(HttpContext context, WebSocket sock) {
            this.webSocket = sock;
            this.Id = context.TraceIdentifier;
        }

        public Task SendText(string txt) {
            var m = Encoding.UTF8.GetBytes(txt);
            return webSocket.SendAsync(m, WebSocketMessageType.Text, true, CancellationToken.None);
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
    }
}
