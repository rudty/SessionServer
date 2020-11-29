using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace SessionServer {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder
                                        .UseKestrel(serverOptions => {
                                            //Set properties and call methods on options
                                            serverOptions.Limits.MaxConcurrentConnections = 100;
                                            serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                                            serverOptions.Limits.MaxRequestBodySize = 10 * 1024;
                                            serverOptions.Limits.MinRequestBodyDataRate =
                                                new MinDataRate(bytesPerSecond: 100,
                                                    gracePeriod: TimeSpan.FromSeconds(10));
                                            serverOptions.Limits.MinResponseDataRate =
                                                new MinDataRate(bytesPerSecond: 100,
                                                    gracePeriod: TimeSpan.FromSeconds(10));
                                            serverOptions.Listen(IPAddress.Loopback, 8080, builder => {
                                                builder.UseConnectionHandler<CPacketConnectionHandler>();
                                            });
                                            serverOptions.ListenLocalhost(5000);
                                            serverOptions.Limits.KeepAliveTimeout =
                                            TimeSpan.FromMinutes(2);
                                            serverOptions.Limits.RequestHeadersTimeout =
                                                TimeSpan.FromMinutes(1);
                                        })
                    .UseStartup<Startup>()
                                        ;
                });
        }

    }
}
