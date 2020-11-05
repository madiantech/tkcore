using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "~", RegName = "HTML",
        Description = "生成RichText控件的HTML")]
    [InstancePlugIn]
    internal class RichTextControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new RichTextControlHtml();

        private RichTextControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.RichText(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}