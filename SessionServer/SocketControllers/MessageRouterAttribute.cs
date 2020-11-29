using System;
using System.Diagnostics.CodeAnalysis;

namespace Sessions.SocketControllers {

    /// <summary>
    /// asp의 [Route] 처럼 선언 시 자동으로 메세지 핸들러에 등록하게함 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageRouterAttribute: Attribute {
        public readonly string Template;

        public MessageRouterAttribute([NotNull] string template) {
            Template = template;
        }

        public MessageRouterAttribute() : this("") {
        }
    }
}
