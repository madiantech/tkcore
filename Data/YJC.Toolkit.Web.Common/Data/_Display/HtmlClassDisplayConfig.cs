using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    [DecorateDisplayConfig(CreateDate = "2015-11-26", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "在显示内容加一层外层并配置以相应的Class或者Style，以达到相应的显示目的")]
    [ObjectContext]
    internal class HtmlClassDisplayConfig : IDecorateDisplay, IConfigCreator<IDecorateDisplay>
    {
        #region IDecorateDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field,
            IFieldValueProvider rowValue, string linkedValue)
        {
            string cssClass = string.IsNullOrEmpty(CssClass) ? string.Empty :
                string.Format(ObjectUtil.SysCulture, " class='{0}'", CssClass);
            string style = string.IsNullOrEmpty(Style) ? string.Empty :
                string.Format(ObjectUtil.SysCulture, " style='{0}'", Style);
            return string.Format(ObjectUtil.SysCulture, "<{0}{1}{2}>{3}</{0}>",
                ElementType.ToString(), cssClass, style, linkedValue);
        }

        #endregion

        #region IConfigCreator<IDecorateDisplay> 成员

        public IDecorateDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        [SimpleAttribute(LocalName = "Class")]
        public string CssClass { get; private set; }

        [SimpleAttribute]
        public string Style { get; private set; }

        [SimpleAttribute(DefaultValue = ContainerElement.Span)]
        public ContainerElement ElementType { get; private set; }
    }
}
