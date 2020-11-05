using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class AssemblyPlugInFactoryAttribute : Attribute
    {
        public Type PlugInFactoryType { get; private set; }

        public AssemblyPlugInFactoryAttribute(Type plugInFactoryType)
        {
            TkDebug.AssertArgumentNull(plugInFactoryType, "plugInFactoryType", null);

            PlugInFactoryType = plugInFactoryType;
            TkDebug.Assert(plugInFactoryType.IsSubclassOf(typeof(BasePlugInFactory)),
                string.Format(ObjectUtil.SysCulture,
                "对象{0}必须从BasePlugInFactory继承，现在不是", plugInFactoryType), null);
        }
    }
}
