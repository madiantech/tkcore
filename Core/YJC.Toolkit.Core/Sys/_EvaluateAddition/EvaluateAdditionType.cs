using System;

namespace YJC.Toolkit.Sys
{
    public abstract class EvaluateAdditionType
    {
        protected EvaluateAdditionType()
        {
        }

        protected abstract Type RegType { get; }

        protected static string GetRegName(Type type)
        {
            EvaluateAdditionAttribute attr = Attribute.GetCustomAttribute(type,
                typeof(EvaluateAdditionAttribute)).Convert<EvaluateAdditionAttribute>();
            TkDebug.AssertNotNull(attr, string.Format(ObjectUtil.SysCulture,
                "{0}类型没有附着EvaluateAdditionAttribute", type), type);
            return attr.GetRegName(type);
        }

        public (string Name, object Value) CreateAdditionObject()
        {
            return (GetRegName(GetType()), RegType);
        }
    }

    public abstract class EvaluateAdditionType<T> : EvaluateAdditionType
    {
        protected EvaluateAdditionType()
        {
        }

        protected override Type RegType
        {
            get
            {
                return typeof(T);
            }
        }
    }
}