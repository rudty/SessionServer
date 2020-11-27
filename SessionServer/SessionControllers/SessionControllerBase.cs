using SessionServer.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SessionServer.SessionControllers {
    public class SessionControllerBase {
        private delegate void ControllerMethod(SessionContext ctx);
        private delegate Task AsyncControllerMethod(SessionContext ctx);
        private static Dictionary<string, ControllerMethod> bindMethods = new Dictionary<string, ControllerMethod>();

        public SessionControllerBase() {
            Type myType = GetType();
            string classRouterTemplate = GetMessageRouterTemplate(myType);
            if (classRouterTemplate == null) {
                classRouterTemplate = "";
            }
            classRouterTemplate += "/";
            
            MethodInfo[] methods = myType.GetMethods();
            foreach(MethodInfo method in methods) {
                string methodTemplate = GetMessageRouterTemplate(method);
                if (methodTemplate != null) {
                    
                }
            }
        }

        private static string GetMessageRouterTemplate(MemberInfo t) {
            if (t.GetCustomAttribute(typeof(MessageRouter)) is MessageRouter m) {
                return m.Template;
            }
            return null;
        }

    }
}
