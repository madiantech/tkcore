using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Razor
{
    [RazorBaseTemplate(RegName = "List", Author = "YJC", CreateDate = "2017-04-20",
        Description = "列表的Razor模板")]
    public class BaseListTemplate : BaseToolkit2Template
    {
        public BaseListTemplate()
        {
            BaseType = typeof(BaseListTemplate);
        }

        protected virtual string RenderRow(IFieldValueProvider provider)
        {
            NormalListData pageData = ViewBag.PageData as NormalListData;
            if (pageData == null)
                return null;

            RazorOutputData output = pageData.RowDisplay;
            if (output == null)
                return null;
            return output.Execute(this, new ProviderModelData(provider));
        }

        protected virtual string RenderQueryItem(IFieldValueProvider provider, IFieldInfoEx field)
        {
            return null;
        }
    }
}