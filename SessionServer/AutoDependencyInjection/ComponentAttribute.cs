using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionServer.AutoDependencyInjection {

    /// <summary>
    /// 선언 시 자동으로 asp.net의 di에서 긁을 수 있게 추가함
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ComponentAttribute : Attribute {
    }
}

