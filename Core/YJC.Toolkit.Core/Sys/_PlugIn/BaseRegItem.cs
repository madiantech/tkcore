using System;

namespace YJC.Toolkit.Sys
{
    internal class BaseRegItem
    {
        protected BaseRegItem(string regName, BasePlugInAttribute attribute)
        {
            RegName = regName;
            Attribute = attribute;
        }

        public string RegName { get; private set; }

        public BasePlugInAttribute Attribute { get; private set; }

        protected void AssertCreateType<T>(Type RegType)
        {
            TkDebug.Assert(ObjectUtil.IsSubType(typeof(T), RegType),
                string.Format(ObjectUtil.SysCulture,
                "注册类型{0}不是参数T类型{1}的子类，无法进行类型转换，请确认",
                RegType, typeof(T)), this);
        }

        public virtual T CreateInstance<T>() where T : class
        {
            return null;
        }

        public virtual T CreateInstance<T>(params object[] args) where T : class
        {
            return null;
        }
    }
}
