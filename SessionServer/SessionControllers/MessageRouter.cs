using System;
using System.Diagnostics.CodeAnalysis;

namespace SessionServer.SessionControllers {

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageRouter: Attribute {
        public readonly string Template;

        public MessageRouter([NotNull] string template) {
            Template = template;
        }

        public MessageRouter() : this("") {
        }
    }
}
