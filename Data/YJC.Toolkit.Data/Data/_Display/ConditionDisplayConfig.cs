using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "根据条件决定使用相应Display进行显示")]
    [ObjectContext]
    internal class ConditionDisplayConfig : IDisplay, IConfigCreator<IDisplay>, IHrefDisplay
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    if (item.IsFitFor(value, rowValue))
                        return DisplayValue(value, field, rowValue, item.Display);
                }
            }

            return DisplayValue(value, field, rowValue, Otherwise);
        }

        #endregion

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        #region IHrefDisplay 成员

        public string Content
        {
            get
            {
                return string.Empty;
            }
            set
            {
                if (Items != null)
                    foreach (var item in Items)
                        DisplayUtil.SetHrefDisplay(item.Display, value);
                DisplayUtil.SetHrefDisplay(Otherwise, value);
            }
        }

        #endregion

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item")]
        public List<ConditionDisplayItemConfig> Items { get; private set; }

        [DynamicElement(CoreDisplayConfigFactory.REG_NAME, Required = true)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IDisplay> Otherwise { get; private set; }

        private static string DisplayValue(object value, Tk5FieldInfoEx field,
            IFieldValueProvider rowValue, IConfigCreator<IDisplay> displayObj)
        {
            var display = displayObj.CreateObject();
            return display.DisplayValue(value, field, rowValue);
        }
    }
}
