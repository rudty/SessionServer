using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionServer.Services.Data {
    public enum SessionStatus {
        Connect,
        Ready,
        Closed,
    }
}
