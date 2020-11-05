using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [RequireAssembly("System.Data.dll", "System.Xml.dll", "System.Xml.Linq.dll", "System.Web.dll",
        "System.Data.DataSetExtensions.dll", "YJC.Toolkit.Web.Common.dll",
        "YJC.Toolkit.MetaData.dll", "YJC.Toolkit.Data.dll", "YJC.Toolkit.AdoData.dll",
        "YJC.Toolkit.Web.Razor.dll", "YJC.Toolkit.Web.Razor2.dll", "YJC.Toolkit.Web.RazorCommon.dll", "YJC.Toolkit.WebApp.dll")]
    [RequireNamespace("YJC.Toolkit.Web")]
    [RazorBaseTemplate(RegName = "Toolkit", Author = "YJC", CreateDate = "2017-04-20",
        Description = "基础Razor模板")]
    public class BaseToolkit2Template : RazorFileTemplate
    {
        private IModelCreator fModelCreator;
        private string fPageTemplateName;

        public BaseToolkit2Template()
        {
            BaseType = typeof(BaseToolkit2Template);
        }

        public override void InitializeTemplate(object configurationData)
        {
            base.InitializeTemplate(configurationData);

            PageTemplateInitData initData = configurationData as PageTemplateInitData;
            if (initData != null)
            {
                fPageTemplateName = initData.PageTemplateName;
                if (string.IsNullOrEmpty(initData.ModelCreatorName))
                    fModelCreator = PageTemplatePlugInFactory.CreateModelCreator(fPageTemplateName);
                else
                    fModelCreator = PlugInFactoryManager.CreateInstance<IModelCreator>(
                        ModelCreatorPlugInFactory.REG_NAME, initData.ModelCreatorName);
            }
        }

        protected virtual string RenderRazorOutputData(RazorOutputData item, object model)
        {
            if (item == null)
                return string.Empty;
            return item.Execute(this, model);
        }

        public string RenderHidden(IFieldValueProvider provider, Tk5FieldInfoEx field, bool needId = true)
        {
            string html = RenderFieldItem(provider, field);
            if (string.IsNullOrEmpty(html))
            {
                return field.Hidden(provider, needId);
            }
            return html;
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

        protected virtual T CreateModel<T>(dynamic model) where T : class, IModel
        {
            TkDebug.AssertNotNull(fModelCreator, string.Format(ObjectUtil.SysCulture,
                "{0}没有设置对应ModelCreator，请确认代码是否正确", fPageTemplateName), this);

            object obj = model;
            return fModelCreator.CreateModel(obj).Convert<T>();
        }
    }
}