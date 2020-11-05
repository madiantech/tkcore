using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static class VueCommonExtension
    {
        private static Dictionary<ControlType, string> fCtrlName = new Dictionary<ControlType, string>
        {
            { ControlType.Text, ControlConst.INPUT },
            { ControlType.Date, ControlConst.DATE },
            { ControlType.DateTime , ControlConst.DATE_TIME },
            { ControlType.Time , ControlConst.TIME },
            { ControlType.Combo , ControlConst.COMBO },
            { ControlType.CheckBox , ControlConst.CHECK_BOX },
            { ControlType.Password , ControlConst.PASSWORD },
            { ControlType.EasySearch , ControlConst.EASY_SEARCH },
            { ControlType.MultipleEasySearch , ControlConst.MULTIPLE_EASY_SEARCH },
            { ControlType.RichText , ControlConst.RICH_TEXT },
            { ControlType.CheckBoxList , ControlConst.CHECK_BOX_LIST },
            { ControlType.Hidden , ControlConst.HIDDEN },
            { ControlType.TextArea , ControlConst.TEXT_AREA },
            { ControlType.Upload , ControlConst.UPLOAD },
            { ControlType.RadioGroup , ControlConst.RADIO_GROUP },
            { ControlType.Label , ControlConst.LABEL }
        };

        public static string VueControlHtml(this Tk5FieldInfoEx field, IPageStyle style,
            string tableName, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, nameof(field), null);
            TkDebug.AssertArgumentNull(style, nameof(style), null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, nameof(tableName), null);

            var ctrl = field.Control?.GetControl(style);
            if (ctrl == null || !fCtrlName.ContainsKey(ctrl.Value))
                return $"<!-- 字段{field.NickName}在{style}下无法获取控件，请确认 -->";
            string ctrlName = fCtrlName[ctrl.Value];

            return CreateControlHtml(field, tableName, provider, needId, ctrlName);
        }

        private static string CreateControlHtml(Tk5FieldInfoEx field, string tableName,
            IFieldValueProvider provider, bool needId, string ctrlName)
        {
            var factory = BaseGlobalVariable.Current.FactoryManager.GetCodeFactory(
                ControlHtmlCreatorPlugInFactory.REG_NAME).Convert<ControlHtmlCreatorPlugInFactory>();
            IControlHtmlCreator htmlCreator = factory.GetHtmlCreator(
                ControlHtmlCreatorConst.TK55_VUE_MODEL, ctrlName);
            if (htmlCreator == null)
                return $"<!-- 在{ControlHtmlCreatorConst.TK55_VUE_MODEL}模式下，没有定义注册名为{ctrlName}的控件 -->";

            return htmlCreator.CreateHtml(tableName, field, provider, needId);
        }

        public static string VueDetailControlHtml(this Tk5FieldInfoEx field,
            string tableName, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, nameof(field), null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, nameof(tableName), null);

            return CreateControlHtml(field, tableName, provider, needId, ControlConst.LABEL);
        }
    }
}