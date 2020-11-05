using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [Serializable]
    public class SimpleFieldControl : IFieldControl, IOrder
    {
        private readonly PageStyle fStyle;

        public SimpleFieldControl(ControlType controlType, int order, PageStyle style)
        {
            SrcControl = controlType;
            Order = order;
            fStyle = style;
        }

        public SimpleFieldControl(IFieldControl control, IPageStyle style)
        {
            fStyle = style.Style;
            Order = control.GetOrder(style);
            SrcControl = control.GetControl(style);
            var customCtrl = control.GetCustomControl(style);
            if (customCtrl != null)
            {
                CustomControl = customCtrl.Item1;
                CustomControlData = customCtrl.Item2;
            }
        }

        #region IFieldControl 成员

        public PageStyle DefaultShow
        {
            get
            {
                return fStyle;
            }
        }

        public ControlType GetControl(IPageStyle style)
        {
            return SrcControl;
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

        [SimpleAttribute]
        public ControlType SrcControl { get; set; }

        [SimpleAttribute]
        public int Order { get; internal set; }

        [SimpleAttribute]
        public string Control { get; set; }

        [SimpleAttribute]
        public string CustomControl { get; set; }

        [SimpleAttribute]
        public string CustomControlData { get; set; }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", SrcControl, Order);
        }
    }
}