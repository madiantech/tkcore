using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class XmlBaseClassAttribute : Attribute
    {
        public XmlBaseClassAttribute(string baseClass, Type regType)
        {
            BaseClass = baseClass;
            RegType = regType;
        }

        public string BaseClass { get; private set; }

        public Type RegType { get; private set; }

        public bool Default { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(BaseClass) || RegType == null ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "名称为{0}的基类型{1}", BaseClass, RegType);
        }
    }
}
