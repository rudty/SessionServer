using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sessions.SocketControllers {
    public static class SocketServiceCollectionExtension {

        /// <summary>
        /// 현재 실행한 어셈블리를 모두 읽어 [SocketController] 가 있는 Attribute 에 대해서
        /// 싱글톤 인스턴스에 추가함
        /// </summary>
        /// <param name="services"></param>
        public static void AddSocketControllers(this IServiceCollection services) {
            var socketControllerClasses = from c in Assembly.GetExecutingAssembly().GetTypes()
                                        where 1 == 1 &&
                                            c.IsClass == true &&
                                            c.BaseType == typeof(SocketControllerBase) &&
                                            c.GetCustomAttribute(typeof(SocketControllerAttribute)) != null
                                        select c;

            foreach (var t in socketControllerClasses) {
                Console.WriteLine("register " + t);
                services.AddSingleton(typeof(SocketControllerBase), t);
            }
        }
    }
}
