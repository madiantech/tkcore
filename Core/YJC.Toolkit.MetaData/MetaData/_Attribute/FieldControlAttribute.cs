using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FieldControlAttribute : Attribute, IFieldControl, IOrder
    {
        public FieldControlAttribute(ControlType control)
        {
            Control = control;
            DefaultShow = PageStyle.All;
        }

        [SimpleAttribute]
        public ControlType Control { get; private set; }

        [SimpleAttribute]
        public int Order { get; set; }

        [SimpleAttribute]
        public string CustomControl { get; set; }

        [SimpleAttribute]
        public string CustomControlData { get; set; }

        #region IFieldControl 成员

        [SimpleAttribute]
        public PageStyle DefaultShow { get; set; }

        public ControlType GetControl(IPageStyle style)
        {
            return Control;
        }

        public int GetOrder(IPageStyle style)
        {
            return Order;
        }

        public Tuple<string, string> GetCustomControl(IPageStyle style)
        {
            return Tuple.Create(CustomControl, CustomControlData);
        }

        #endregion IFieldControl 成员

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", Control, Order);
        }
    }
}