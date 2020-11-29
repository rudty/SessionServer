using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sessions.SocketControllers {
    public static class SocketApplicationBuilderExtension {
        public static void UseSocketRouting(this IApplicationBuilder app) {
            // TODO 일단 GetServices 호출 시 즉시 생성자 동작이 가능하긴하지만
            // 나중에 게으른 처리를 하게 바꾸도록 합시다
            IServiceProvider serviceProvider = app.ApplicationServices;
            IEnumerable<SocketControllerBase> b =  serviceProvider.GetServices<SocketControllerBase>();

        }
    }
}
