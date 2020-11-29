using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace SessionServer.Sessions {

    /// <summary>
    /// 소켓 접속 1 개당 세션 1 개
    /// </summary>
    public class SocketContext {
        private readonly Stream outputStream;
        
        public object UserData { get; set; } = null;
        public CPacket InputPacket { get; internal set; }

        public string Message { get; internal set; }

        public SocketContext(Stream outputStream) {
            this.outputStream = outputStream;
        }
    }
}
