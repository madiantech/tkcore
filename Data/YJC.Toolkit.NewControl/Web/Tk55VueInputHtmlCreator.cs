using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    //[InstancePlugIn, AlwaysCache]
    //[ControlHtmlCreator(ControlHtmlCreatorConst.TK55_VUE_MODEL, RegName = ControlConst.INPUT,
    //    CreateDate = "2020-09-21", Author = "YJC", Description = DESCRIPTION)]
    internal class Tk55VueInputHtmlCreator : BaseVueControlHtmlCreator
    {
        private const string DESCRIPTION = "生成传统的Text控件";
        public static readonly IControlHtmlCreator Instance = new Tk55VueInputHtmlCreator();

        private Tk55VueInputHtmlCreator()
        {
        }

        public override string CreateHtml(string tableName, Tk5FieldInfoEx field,
            IFieldValueProvider provider, bool needId)
        {
            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            builder.AddNormalAttribute(field)
                .AddVModelAttribute(field, tableName, needId);

            return $"<tk-input {builder}/>";
        }

        public override string ToString() => DESCRIPTION;
    }
}