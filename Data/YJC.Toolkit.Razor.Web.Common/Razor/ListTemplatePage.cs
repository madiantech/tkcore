using Microsoft.AspNetCore.Html;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Razor
{
    public abstract class ListTemplatePage<TModel> : ToolkitTemplatePage<TModel>
    {
        protected ListTemplatePage()
        {
        }

        protected virtual HtmlString RenderRow(IFieldValueProvider provider)
        {
            NormalListData pageData = ViewBag.PageData as NormalListData;
            if (pageData == null)
                return HtmlString.Empty;

            RazorOutputData output = pageData.RowDisplay;
            if (output == null)
                return HtmlString.Empty;
            return output.Execute(this, new ProviderModelData(provider));
        }

        protected virtual HtmlString RenderQueryItem(IFieldValueProvider provider, IFieldInfoEx field)
        {
            return HtmlString.Empty;
        }
    }
}