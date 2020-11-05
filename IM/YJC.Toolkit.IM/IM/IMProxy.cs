using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public class IMProxy<T> : DispatchProxy
    {
        private readonly InterfaceStructure fStructure;
        private IIMPlatform fPlatform;

        public IMProxy()
        {
            fStructure = InterfaceStructure.Create(typeof(T));
        }

        internal void SetProperties(IIMPlatform platform)
        {
            TkDebug.AssertArgumentNull(platform, nameof(platform), null);

            fPlatform = platform;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return fStructure.Execute(fPlatform, targetMethod, args);
        }

        public override string ToString() => $"{fPlatform}服务代理对象";
    }
}