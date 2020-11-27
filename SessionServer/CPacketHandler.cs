using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionServer {
    public class CPacketHandler : ConnectionHandler {

        private readonly ILogger<CPacketHandler> logger;

        public CPacketHandler(ILogger<CPacketHandler> logger) {
            this.logger = logger;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection) {
            logger.LogInformation(connection.ConnectionId + " connected");
            byte[] buf = new byte[1024];
            var inputStream = connection.Transport.Input.AsStream();
            var outputStream = connection.Transport.Output.AsStream();
            
            while (true) {
                int len = await inputStream.ReadAsync(buf, 0, buf.Length);
                if (len == 0) {
                    break;
                }

                Console.WriteLine(Encoding.UTF8.GetString(buf,0, len));
                await outputStream.WriteAsync(buf, 0, len);
            }

            logger.LogInformation(connection.ConnectionId + " disconnected");
        }
    }
}
