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

        public void Register(Session s) {
            if (false == sessions.TryAdd(s.Id, s)) {
                throw new Exception($"{s.Id} exists");
            }
        }

        public bool Unregister(Session s) => Unregister(s.Id);

        public bool Unregister(string sessionId) {
            if (sessions.TryRemove(sessionId, out Session _)) {
                return true;
            }
            return false;
        }

        public bool Exists(Session s) => Exists(s.Id);
        public bool Exists(string id) => Get(id) != null;

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
