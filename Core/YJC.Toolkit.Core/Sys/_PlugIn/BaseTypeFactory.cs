using System;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseTypeFactory : BasePlugInFactory
    {
        protected BaseTypeFactory(string name, string description)
            : base(name, description)
        {
        }

        public Type GetType(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);

            CodeRegItem regItem = GetRegItem(regName) as CodeRegItem;
            if (regItem != null)
                return regItem.RegType;

            return null;
        }
    }
}
