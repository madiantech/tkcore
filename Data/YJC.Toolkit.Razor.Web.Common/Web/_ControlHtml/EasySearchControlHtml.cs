using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02",
        RangeControl = "EasySearchRange", Description = "生成EasySearch控件的HTML")]
    [InstancePlugIn]
    internal class EasySearchControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new EasySearchControlHtml();

        private EasySearchControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.EasySearch(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}