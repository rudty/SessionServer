using SocketServer.Core;
using SocketServer.Net.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SessionServer.Sessions {

    /// <summary>
    /// 소켓 접속 1 개당 세션 1 개
    /// </summary>
    public class SessionContext {
        private readonly Stream inputStream;
        private readonly Stream outputStream;

        public object UserData { get; set; } = null;
        public SessionContext(Stream inputStream, Stream outputStream) {
            this.inputStream = inputStream;
            this.outputStream = outputStream;
        }

        public async Task Run() {
            CPacketStreamReader reader = new CPacketStreamReader(outputStream);
            while (true) {
                CPacket p = await reader.ReceiveAsync();
                if (p != null) {
                    
                }
            }
        }
    }
}
