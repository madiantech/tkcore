using System;

namespace YJC.Toolkit.Sys
{
    internal class CodeRegItem : BaseRegItem
    {
        public CodeRegItem(string regName, BasePlugInAttribute attribute, Type regType)
            : base(regName, attribute)
        {
            RegType = regType;
        }

        public Type RegType { get; private set; }

        public override T CreateInstance<T>()
        {
            object result = ObjectUtil.CreateObject(RegType);
            AssertCreateType<T>(RegType);

            return result as T;
        }

        public override T CreateInstance<T>(params object[] args)
        {
            object result = ObjectUtil.CreateObject(RegType, args);
            AssertCreateType<T>(RegType);

            return result as T;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(RegName) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "注册名为{0}的Code配置单元", RegName);
        }
    }
}
