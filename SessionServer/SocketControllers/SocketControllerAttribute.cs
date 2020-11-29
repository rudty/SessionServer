using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sessions.SocketControllers {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SocketControllerAttribute: Attribute {
    }
}
