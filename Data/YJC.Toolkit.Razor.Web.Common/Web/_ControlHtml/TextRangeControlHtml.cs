﻿using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "~",
        Description = "生成区间Text控件的HTML")]
    [InstancePlugIn]
    internal class TextRangeControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new TextRangeControlHtml();

        private TextRangeControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            string startHtml = field.Input(provider, true);
            string endHtml = HtmlCommonExtension.InputEnd(field, provider);
            return HtmlCommonUtil.GetRangeCtrl(startHtml, endHtml);
        }

        #endregion IControlHtml 成员
    }
}