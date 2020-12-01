using SessionServer.Services.Data;

namespace SessionServer.Controllers.Transfer {
    public readonly struct InformationResponse {
        public bool Exist { get; }

        public SessionTransfer Session { get; }

        public InformationResponse(Session session) {
            if (session != null) {
                Exist = true;
                Session = new SessionTransfer(session);
            } else {
                Exist = false;
                Session = SessionTransfer.Empty;
            }
        }
    }
}
