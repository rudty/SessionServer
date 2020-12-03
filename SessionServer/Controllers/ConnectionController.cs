using Microsoft.AspNetCore.Mvc;
using SessionServer.Services;
using SessionServer.Services.Data;
using SessionServer.Controllers.Transfer;
namespace SessionServer.Controllers {
    [Route("api/[controller]")]
    public class ConnectionController : Controller {

        private readonly SessionService sessionService;

        public ConnectionController(SessionService sessionService) {
            this.sessionService = sessionService;
        }

        [HttpGet("information/{sessionId}")]
        public InformationResponse Exist(string sessionId) {
            Session session = sessionService.Get(sessionId);
            return new InformationResponse(session);
        }

        [HttpGet("kick/{sessionId}")]
        public KickResponse Kick(string sessionId) {
            Session session = sessionService.Get(sessionId);
            if (session != null) {
                session.Close();
                return new KickResponse(true);
            }
            return new KickResponse(false);
        }
    }
}
