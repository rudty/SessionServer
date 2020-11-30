using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SessionServer.Controllers;
using SessionServerTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SessionServer.Controllers.Tests {
    [TestClass()]
    public class WebSocketControllerTests {

        [TestMethod()]
        public async Task IndexTest() {
            using var testWebSocket = new TestWebSocket() {
                Path = "api/WebSocket",
            };
            await testWebSocket.Connect<Startup>();

            string s = await testWebSocket.ReceiveString();
            Assert.AreEqual(s, "serverhello");
        }
    }
}