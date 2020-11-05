using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "Text",
        RangeControl = "TextRange", Description = "生成Detail控件的HTML")]
    [InstancePlugIn]
    internal class LabelControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new LabelControlHtml();

        private LabelControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.Detail(provider, false, needId);
        }

        #endregion IControlHtml 成员
    }
}