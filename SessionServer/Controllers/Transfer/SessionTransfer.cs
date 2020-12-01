using SessionServer.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionServer.Controllers.Transfer {
    public readonly struct SessionTransfer {

        public static SessionTransfer Empty { get; } = new SessionTransfer {
            Id = "",
            LoginTime = DateTime.MinValue,
            SessionStatus = SessionStatus.Closed,
        };

        public string Id { get; init; }
        public DateTime LoginTime { get; init; }
        public SessionStatus SessionStatus { get; init; }

        public SessionTransfer(Session session) {
            Id = session.Id;
            LoginTime = session.LoginTime;
            SessionStatus = session.SessionStatus;
        }
    }
}
