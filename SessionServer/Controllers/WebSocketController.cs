﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SessionServer.Services;
using SessionServer.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SessionServer.Controllers {
    [Route("api/[controller]")]
    public class WebSocketController : Controller {

        private readonly SocketMessageService socketMessageService;
        private readonly SessionService sessionService;
        private readonly ILogger<WebSocketController> logger;

        public WebSocketController(
                ILogger<WebSocketController> logger, 
                SessionService sessionService,
                SocketMessageService socketMessageService) {

            this.sessionService = sessionService;
            this.logger = logger;
            this.socketMessageService = socketMessageService;
        }

        [HttpGet]
        public async Task Index() {
            HttpContext context = ControllerContext.HttpContext;
            WebSocketManager websocketManager = context.WebSockets;
            
            if (!websocketManager.IsWebSocketRequest) {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }
            
            using WebSocket webSocket = await websocketManager.AcceptWebSocketAsync();
            Session session = new Session(context, webSocket);
            sessionService.Register(session);
            logger.LogInformation($"connect session {session.Id}");

            await socketMessageService.HandleMessage(session, webSocket);

            logger.LogInformation($"disconnect session {session.Id}");
            sessionService.Unregister(session);
        }
    }
}
