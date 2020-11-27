using SocketServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionServer {
    public class PacketInputReader {

        CPacket packet = new CPacket();

        public void acc(ReadOnlyMemory<byte> m) {
            ReadOnlySpan<byte> span = m.Span;
            //span[0] != CPacket.PACKET_BEGIN
        } 
    }
}
