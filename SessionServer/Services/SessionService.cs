using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using SessionServer.AutoDependencyInjection;
using SessionServer.Services.Data;
using Microsoft.AspNetCore.Http;

namespace SessionServer.Services {

    [Component]
    public class SessionService {
        ConcurrentDictionary<string, Session> sessions = new ConcurrentDictionary<string, Session>();

        public Session Register(HttpContext ctx, WebSocket socket) {
            Session s = new Session(ctx, socket);
            if (sessions.TryAdd(s.Id, s)) {
                return s;
            }
            throw new Exception($"{s.Id} exists");
        }

        public Session Unregister(HttpContext ctx) {
            string id = ctx.Connection.Id;
            if (sessions.TryRemove(id, out Session session)) {
                return session;
            }
            throw new Exception($"{id} not exists");
        }

        public Session Get(string id) {
            if (sessions.TryGetValue(id, out Session session)) {
                return session;
            }
            return null;
        }

        public Session this[string id] {
            get {
                return Get(id);
            }
        }
    }

}
