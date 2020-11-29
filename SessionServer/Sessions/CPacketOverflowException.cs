using System;

namespace SessionServer.Sessions {
    public class CPacketOverflowException : OverflowException {
        public CPacketOverflowException(string message) : base(message) {
        }
    }
}
