using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Razor
{
    [RequireAssembly("System.Data.dll", "System.Xml.dll", "System.Xml.Linq.dll", "System.Web.dll",
        "System.Data.DataSetExtensions.dll",
        "YJC.Toolkit.MetaData.dll", "YJC.Toolkit.Data.dll", "YJC.Toolkit.AdoData.dll",
        "YJC.Toolkit.Web.Razor.dll", "YJC.Toolkit.Web.RazorCommon.dll", "YJC.Toolkit.WebApp.dll")]
    [RequireNamespace("YJC.Toolkit.Web")]
    public abstract class BaseToolkitTemplate : RazorFileTemplate
    {
        protected BaseToolkitTemplate()
        {
        }

        protected virtual string RenderRazorOutputData(RazorOutputData item, object model)
        {
            if (item == null)
                return string.Empty;
            return item.Execute(this, model);
        }

        protected virtual string RenderFieldItem(DataRow row, IFieldInfoEx field)
        {
            IRazorField pageData = ViewBag.PageData as IRazorField;
            if (pageData == null)
                return null;

            RazorField razorField = pageData.GetDisplayField(field);
            if (razorField == null)
                return null;
            return razorField.Execute(this, new FieldModelData(row, field));
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

        protected virtual string RenderFieldItem(DataRow row, string tableName, IFieldInfoEx field)
        {
            IRazorField pageData = ViewBag.PageData as IRazorField;
            if (pageData == null)
                return null;

            RazorField razorField = pageData.GetDisplayField(tableName, field);
            if (razorField == null)
                return null;
            return razorField.Execute(this, new FieldModelData(row, field));
        }

        protected virtual string RenderFieldItem(IFieldValueProvider provider, string tableName, IFieldInfoEx field)
        {
            IRazorField pageData = ViewBag.PageData as IRazorField;
            if (pageData == null)
                return null;

            RazorField razorField = pageData.GetDisplayField(tableName, field);
            if (razorField == null)
                return null;
            return razorField.Execute(this, new FieldProviderModelData(provider, field));
        }
    }
}