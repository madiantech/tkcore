using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class VueHtmlBuilderExtension
    {
        private static HtmlAttribute AddAttributeIfExist(Tk5FieldInfoEx field, string attrName)
        {
            var attrList = field.Edit?.AttributeList;
            if (attrList == null)
                return null;

            var attr = (from item in attrList
                        where item.Name == attrName
                        select item).FirstOrDefault();
            if (attr == null)
                return null;

            string newAttrName = char.ToLower(attrName[0]) + attrName.Substring(1);
            return new HtmlAttribute(newAttrName, attr.Value);
        }

        public static HtmlAttributeBuilder AddAttributesIfExist(this HtmlAttributeBuilder builder,
            Tk5FieldInfoEx field, params string[] attrNames)
        {
            if (attrNames == null)
                return builder;

            foreach (var attrName in attrNames)
                builder.Add(AddAttributeIfExist(field, attrName));

            return builder;
        }

        public static HtmlAttributeBuilder AddBaseAttribute(this HtmlAttributeBuilder builder,
            Tk5FieldInfoEx field, bool needId)
        {
            builder.Add("name", field.NickName);
            builder.Add("caption", field.DisplayName);
            if (!needId)
            {
                builder.Add("notform", null);
                builder.Add("slot-scope", "{row}");
            }

            return builder;
        }

        public static HtmlAttributeBuilder AddNormalAttribute(this HtmlAttributeBuilder builder,
            Tk5FieldInfoEx field)
        {
            if (field.Edit?.ReadOnly == true)
                builder.Add("readonly", null);
            if (!field.IsEmpty)
                builder.Add("required", null);
            string clsName = field.Edit?.Class;
            if (clsName != null)
                builder.Add("classname", clsName);

            return builder;
        }

        public static HtmlAttributeBuilder AddVModelAttribute(this HtmlAttributeBuilder builder,
            Tk5FieldInfoEx field, string tableName, bool needId)
        {
            string row = needId ? tableName : "row";
            var attr = new HtmlAttribute(ControlHtmlCreatorConst.V_MODEL,
                $"{row}.{field.NickName}");
            builder.Add(attr);
            return builder;
        }

        public static HtmlAttributeBuilder AddPlaceholder(this HtmlAttributeBuilder builder,
            Tk5FieldInfoEx field)
        {
            if (!string.IsNullOrEmpty(field.Hint))
            {
                if (field.HintPosition == HintPosition.PlaceHolder)
                    builder.Add("placeholder", field.Hint);
            }

            return builder;
        }

        public static HtmlAttributeBuilder SetCheckedValue(this HtmlAttributeBuilder builder,
            Tk5FieldInfoEx field)
        {
            if (field.Extension != null)
            {
                if (!string.IsNullOrEmpty(field.Extension.CheckValue))
                    builder.Add("onValue", field.Extension.CheckValue);
                if (!string.IsNullOrEmpty(field.Extension.UnCheckValue))
                    builder.Add("offValue", field.Extension.UnCheckValue);
            }

            return builder;
        }
    }
}