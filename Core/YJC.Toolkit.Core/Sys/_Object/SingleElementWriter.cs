using System;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    internal class SingleElementWriter : IElementWriter
    {
        private readonly NamedAttribute fAttribute;

        public SingleElementWriter(ObjectPropertyInfo propertyInfo)
        {
            Content = propertyInfo;
            fAttribute = Content.Attribute.Convert<NamedAttribute>();
            SimpleElementAttribute eleAttr = fAttribute as SimpleElementAttribute;
            if (eleAttr != null)
                IsValueMulitple = eleAttr.IsMultiple;
            IOrder order = fAttribute as IOrder;
            Order = order != null ? order.Order : int.MaxValue;
            PropertyName = propertyInfo.PropertyName;
        }

        #region IElementWriter 成员

        public int Order { get; private set; }

        public bool IsSingle
        {
            get
            {
                return true;
            }
        }

        public bool IsValueMulitple { get; private set; }

        public ObjectPropertyInfo Content { get; private set; }

        public bool Required
        {
            get
            {
                return fAttribute.Required;
            }
        }


        public string PropertyName { get; private set; }
        

        public ObjectPropertyInfo Get(Type type)
        {
            throw new NotSupportedException();
        }

        public object GetValue(object receiver)
        {
            return Content.GetValue(receiver);
        }


        #endregion IElementWriter 成员
    }
}