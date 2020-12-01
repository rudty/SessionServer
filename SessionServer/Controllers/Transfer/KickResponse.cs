using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionServer.Controllers.Transfer {
    public readonly struct KickResponse {
        public bool Ok { get; }

        public KickResponse(bool ok) {
            Ok = ok;
        }
    }
}
