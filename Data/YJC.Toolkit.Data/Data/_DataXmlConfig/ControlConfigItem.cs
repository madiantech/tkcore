using System;
using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class ControlConfigItem : IFieldControl, IReadObjectCallBack, IOrder
    {
        private Dictionary<PageStyleClass, PageConfigItem> fDictionary;

        [SimpleAttribute(Required = true)]
        public ControlType Control { get; private set; }

        [SimpleAttribute(Required = true)]
        public int Order { get; private set; }

        [SimpleAttribute(Required = true)]
        public PageStyle DefaultShow { get; private set; }

        [SimpleAttribute]
        public string CustomControl { get; private set; }

        [SimpleAttribute]
        public string CustomControlData { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Page")]
        public List<PageConfigItem> PageList { get; private set; }

        #region IFieldControl 成员

        public ControlType GetControl(IPageStyle style)
        {
            PageConfigItem item = GetConfigItem(style);
            if (item == null)
                return Control;
            else
                return item.Control;
        }

        public int GetOrder(IPageStyle style)
        {
            PageConfigItem item = GetConfigItem(style);
            if (item == null || item.Order == 0)
                return Order;
            else
                return item.Order;
        }

        public Tuple<string, string> GetCustomControl(IPageStyle style)
        {
            PageConfigItem item = GetConfigItem(style);
            string ctrl = CustomControl;
            string ctrlData = CustomControlData;
            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.CustomControl))
                    ctrl = item.CustomControl;
                if (!string.IsNullOrEmpty(item.CustomControlData))
                    ctrlData = item.CustomControlData;
            }
            return Tuple.Create(ctrl, ctrlData);
        }

        #endregion IFieldControl 成员

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (PageList != null)
            {
                fDictionary = new Dictionary<PageStyleClass, PageConfigItem>();
                foreach (var item in PageList)
                    fDictionary.Add(item.Style, item);
            }
        }

        #endregion IReadObjectCallBack 成员

        private PageConfigItem GetConfigItem(IPageStyle style)
        {
            if (fDictionary == null)
                return null;

            PageStyleClass styleClass = PageStyleClass.FromStyle(style);
            PageConfigItem result;
            if (fDictionary.TryGetValue(styleClass, out result))
                return result;
            return null;
        }
    }
}