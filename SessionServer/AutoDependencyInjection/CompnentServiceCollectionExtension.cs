using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SessionServer.AutoDependencyInjection {
    public static class CompnentServiceCollectionExtension {

        /// <summary>
        /// 현재 실행한 어셈블리를 모두 읽어 [SocketController] 가 있는 Attribute 에 대해서
        /// 싱글톤 인스턴스에 추가함
        /// ConfigureServices 에서 호출할 것
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection 서비스</param>
        public static void AddAllComponent(this IServiceCollection services) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var componentClasses = from c in assembly.GetTypes()
                                          where 
                                              c.IsClass == true &&
                                              c.GetCustomAttribute(typeof(ComponentAttribute)) != null
                                          select c;

            foreach (Type c in componentClasses) {
                services.AddSingleton(c);
            }
        }
    }
}
