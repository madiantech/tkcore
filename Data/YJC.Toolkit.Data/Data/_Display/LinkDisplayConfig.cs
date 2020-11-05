using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "链接一个核心数据显示和多个装饰Display")]
    [ObjectContext]
    internal class LinkDisplayConfig : IDisplay, IConfigCreator<IDisplay>, IHrefDisplay
    {
        public LinkDisplayConfig()
        {
        }

        public LinkDisplayConfig(IConfigCreator<IDisplay> core, IConfigCreator<IDecorateDisplay> decorate)
        {
            CoreDisplay = core;
            DecorateDisplays = new List<IConfigCreator<IDecorateDisplay>> { decorate };
        }

        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            IDisplay display = CoreDisplay.CreateObject();
            if (display == null)
                return string.Empty;
            string linkValue = display.DisplayValue(value, field, rowValue);
            if (DecorateDisplays == null)
                return linkValue;
            foreach (var item in DecorateDisplays)
            {
                var decorateDisplay = item.CreateObject();
                if (decorateDisplay == null)
                    continue;
                linkValue = decorateDisplay.DisplayValue(value, field, rowValue, linkValue);
            }

            return linkValue;
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
                DisplayUtil.SetHrefDisplay(CoreDisplay, value);
                foreach (var item in DecorateDisplays)
                    DisplayUtil.SetHrefDisplay(item, value);
            }
        }

        #endregion

        [DynamicElement(CoreDisplayConfigFactory.REG_NAME, Required = true)]
        public IConfigCreator<IDisplay> CoreDisplay { get; internal set; }

        [DynamicElement(DecorateDisplayConfigFactory.REG_NAME, IsMultiple = true, Required = true)]
        public List<IConfigCreator<IDecorateDisplay>> DecorateDisplays { get; internal set; }
    }
}
