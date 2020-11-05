using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-01", SearchControl = "Text",
        RangeControl = "TextRange", Description = "生成TextArea控件的HTML")]
    [InstancePlugIn]
    internal class TextAreaControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new TextAreaControlHtml();

        private TextAreaControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            return field.Textarea(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}