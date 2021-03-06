﻿using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02",
        RangeControl = "ComboRange", Description = "生成Combo控件的HTML")]
    [InstancePlugIn]
    internal class ComboControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new ComboControlHtml();

        private ComboControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.Combo(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}