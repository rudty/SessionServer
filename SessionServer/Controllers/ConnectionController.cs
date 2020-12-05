using Microsoft.AspNetCore.Mvc;
using SessionServer.Services;
using SessionServer.Services.Data;
using SessionServer.Controllers.Transfer;
using Microsoft.AspNetCore.Http;

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

        [HttpPost("kick/{sessionId}")]
        public KickResponse Kick(string sessionId, string reason) {
            //string reason = context.Request.Query["reason"];
            System.Console.WriteLine(reason);
            System.Console.WriteLine(sessionId);
            Session session = sessionService.Get(sessionId);
            if (session != null) {
                session.Close();
                return new KickResponse(true);
            }
            return new KickResponse(false);
        }
    }
}
