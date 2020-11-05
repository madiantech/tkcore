using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    [AlwaysCache]
    internal class InterfaceStructure
    {
        private readonly Dictionary<MethodInfo, MethodDataInfo> fMethods;
        private readonly Type fIntfType;

        internal InterfaceStructure(Type intfType)
        {
            fIntfType = intfType;
            fMethods = new Dictionary<MethodInfo, MethodDataInfo>();
            foreach (var method in intfType.GetMethods())
            {
                ApiMethodAttribute attr = Attribute.GetCustomAttribute(method,
                    typeof(ApiMethodAttribute)) as ApiMethodAttribute;
                if (attr != null)
                {
                    MethodDataInfo info = new MethodDataInfo(method, attr);
                    fMethods.Add(method, info);
                }
            }
        }

        public MethodDataInfo GetMethodInfo(MethodInfo method)
        {
            if (fMethods.ContainsKey(method))
                return fMethods[method];

            return null;
        }

        public object Execute(IIMPlatform platform, MethodInfo method, object[] args)
        {
            MethodDataInfo info = GetMethodInfo(method);
            TkDebug.AssertNotNull(info, string.Format(ObjectUtil.SysCulture,
                "{0}中的方法{1}没有标注ApiMethod特性，无法获取其元数据",
                method.DeclaringType, method.Name), this);

            return info.Execute(platform, args);
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "接口类型{0}", fIntfType);
        }

        public static InterfaceStructure Create(Type intfType)
        {
            TkDebug.AssertArgumentNull(intfType, "intfType", null);

            string key = intfType.ToString();
            return CacheManager.GetItem("InterfaceStructure",
                key, intfType).Convert<InterfaceStructure>();
        }
    }
}