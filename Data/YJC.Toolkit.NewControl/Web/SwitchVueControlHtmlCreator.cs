using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtmlCreator(ControlHtmlCreatorConst.TK55_VUE_MODEL, RegName = ControlConst.SWITCH)]
    internal class SwitchVueControlHtmlCreator : BaseVueControlHtmlCreator
    {
        public override string CreateHtml(string tableName, Tk5FieldInfoEx field,
            IFieldValueProvider provider, bool needId)
        {
            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            builder.AddBaseAttribute(field, needId)
                .AddVModelAttribute(field, tableName, needId)
                .AddNormalAttribute(field)
                .AddAttributesIfExist(field, "OnText", "OffText", "OnColor", "OffColor", "Tooltip")
                .SetCheckedValue(field);
            return $"<tk-switch {builder.CreateAttribute()}></tk-switch>";
        }
    }
}