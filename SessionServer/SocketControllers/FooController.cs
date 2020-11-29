using Sessions.Sessions;
using Sessions.SocketControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sessions.SocketControllers {

    [SocketController]
    public class FooController: SocketControllerBase {
        public FooController() {
            Console.WriteLine("init foo");
        }

        [MessageRouter("hello")]
        public Task Bar(SocketContext ctx) {
            return Task.CompletedTask;
        }
    }
}
