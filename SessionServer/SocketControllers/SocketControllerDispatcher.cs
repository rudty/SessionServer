using SessionServer.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;

namespace Sessions.SocketControllers {
    public class SocketControllerDispatcher {
        private delegate void SyncControllerMethod(SocketContext ctx);
        private delegate Task AsyncControllerMethod(SocketContext ctx);

        public static SocketControllerDispatcher Instance { get; private set; } = new SocketControllerDispatcher();


        private Dictionary<string, AsyncControllerMethod> bindMethods = new Dictionary<string, AsyncControllerMethod>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public void Register(SocketControllerBase c) {
            Type myType = GetType();
            string classRouterTemplate = GetMessageRouterTemplate(myType);
            if (classRouterTemplate == null) {
                classRouterTemplate = "";
            }

            if (classRouterTemplate.Length > 0 &&
                false == classRouterTemplate.EndsWith("/")) {
                classRouterTemplate += "/";
            }

            MethodInfo[] methods = myType.GetMethods();
            foreach (MethodInfo method in methods) {
                string methodTemplate = GetMessageRouterTemplate(method);
                if (methodTemplate != null) {
                    string routeTemplate = classRouterTemplate + methodTemplate;
                    AsyncControllerMethod bindMethod;

                    if (method.ReturnType == typeof(void)) {
                        bindMethod = SyncToATaskMethod(c, method);
                    } else if (method.ReturnType == typeof(Task)) {
                        bindMethod = TaskMethod(c, method);
                    } else {
                        throw new Exception(" (void | Task) AsyncControllerMethod(SessionContext ctx);");
                    }
                    lock (bindMethods) {
                        bindMethods.Add(routeTemplate, bindMethod);
                    }
                }
            }
        }

        /// <summary>
        /// [MessageRouter] 가 붙은 클래스의 어트리뷰트 값을 가져옴 
        /// </summary>
        /// <param name="t">메서드 </param>
        /// <returns>있다면 string 값 없다면 null</returns>
        private static string GetMessageRouterTemplate(MemberInfo t) {
            if (t.GetCustomAttribute(typeof(MessageRouterAttribute)) is MessageRouterAttribute m) {
                return m.Template;
            }
            return null;
        }

        private static T CreateDelegate<T>(object o, MethodInfo method) where T: class {
            return Delegate.CreateDelegate(
                typeof(SyncControllerMethod),
                o,
                method) as T;
        }

        private static AsyncControllerMethod SyncToATaskMethod(SocketControllerBase c, MethodInfo method) {
            SyncControllerMethod l = CreateDelegate<SyncControllerMethod>(c, method);
            return (SocketContext ctx) => {
                l(ctx);
                return Task.CompletedTask;
            };
        }

        private static AsyncControllerMethod TaskMethod(SocketControllerBase c, MethodInfo method) {
            return CreateDelegate<AsyncControllerMethod>(c, method);
        }

        public Task Dispatch(SocketContext ctx) {
            string message = ctx.Message;
            AsyncControllerMethod c;
            if (bindMethods.TryGetValue(message, out c)) {
                return c(ctx);
            }
            return Task.CompletedTask;
        }
    }
}
