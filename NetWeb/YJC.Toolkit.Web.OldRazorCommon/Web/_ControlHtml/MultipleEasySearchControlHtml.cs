using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "~",
        Description = "生成MultipleEasySearch控件的HTML")]
    [InstancePlugIn]
    internal class MultipleEasySearchControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new MultipleEasySearchControlHtml();

        private MultipleEasySearchControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.MultipleEasySearch(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}