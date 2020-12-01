using SessionServer.Services.Data;

namespace SessionServer.Controllers.Transfer {
    public readonly struct InformationResponse {
        public bool Exist { get; }

        public Session Session { get; }

        public InformationResponse(Session session) {
            Exist = (session != null);
            Session = session;
        }
    }
}
