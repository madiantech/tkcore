using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Razor
{
    [RequireAssembly("System.Data.dll", "System.Xml.dll", "System.Xml.Linq.dll", "System.Web.dll",
       "System.Data.DataSetExtensions.dll", "YJC.Toolkit.Core.Extension.dll",
       "YJC.Toolkit.MetaData.dll", "YJC.Toolkit.Data.dll", "YJC.Toolkit.AdoData.dll",
       "YJC.Toolkit.Web.Razor.dll", "YJC.Toolkit.Web.RazorCommon.dll", "YJC.Toolkit.WebApp.dll")]
    [RequireNamespace("YJC.Toolkit.Web")]
    public abstract class BaseToolkitObjectTemplate : RazorFileTemplate
    {
        protected BaseToolkitObjectTemplate()
        {
        }

        protected virtual string RenderRazorOutputData(RazorOutputData item, object model)
        {
            if (item == null)
                return string.Empty;
            return item.Execute(this, model);
        }

        protected virtual string RenderFieldItem(ObjectContainer receiver, IFieldInfoEx field)
        {
            IRazorField pageData = ViewBag.PageData as IRazorField;
            if (pageData == null)
                return null;

            RazorField razorField = pageData.GetDisplayField(field);
            if (razorField == null)
                return null;
            return razorField.Execute(this, new FieldObjectModelData(receiver, field));
        }

        protected virtual string RenderFieldItem(IFieldValueProvider provider, IFieldInfoEx field)
        {
            IRazorField pageData = ViewBag.PageData as IRazorField;
            if (pageData == null)
                return null;

            RazorField razorField = pageData.GetDisplayField(field);
            if (razorField == null)
                return null;
            return razorField.Execute(this, new FieldProviderModelData(provider, field));
        }
    }
}