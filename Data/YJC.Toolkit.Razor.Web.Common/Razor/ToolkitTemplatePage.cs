using Microsoft.AspNetCore.Html;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public abstract class ToolkitTemplatePage<TModel> : TemplatePage<TModel>
    {
        private IModelCreator fModelCreator;
        private string fPageTemplateName;

        protected ToolkitTemplatePage()
        {
        }

        public override PageContext PageContext
        {
            get => base.PageContext;
            set
            {
                base.PageContext = value;
                if (value?.InitData is IPageTemplateInitData initData)
                {
                    fPageTemplateName = initData.PageTemplateName;
                    if (string.IsNullOrEmpty(initData.ModelCreatorName))
                        fModelCreator = PageTemplatePlugInFactory.CreateModelCreator(fPageTemplateName);
                    else
                        fModelCreator = PlugInFactoryManager.CreateInstance<IModelCreator>(
                            ModelCreatorPlugInFactory.REG_NAME, initData.ModelCreatorName);
                }
            }
        }

        protected virtual HtmlString RenderRazorOutputData(RazorOutputData item, object model)
        {
            if (item == null)
                return HtmlString.Empty;
            return item.Execute(this, model);
        }

        public string RenderHidden(IFieldValueProvider provider, Tk5FieldInfoEx field, bool needId = true)
        {
            HtmlString html = RenderFieldItem(provider, field);
            if (html == null)
                return field.Hidden(provider, needId);
            return html.Value;
        }

        public string RenderHidden(IFieldValueProvider provider, string tableName, Tk5FieldInfoEx field, bool needId = true)
        {
            HtmlString html = RenderFieldItem(provider, tableName, field);
            if (html == null)
                return field.Hidden(provider, needId);
            return html.Value;
        }

        public string MakeRelativePath(string path)
        {
            TkDebug.AssertArgumentNullOrEmpty(path, nameof(path), this);

            return RazorUtil.MakeRelativePath(Key, path);
        }

        protected override string PrepareKey(string key)
        {
            TkDebug.AssertArgumentNullOrEmpty(key, nameof(key), this);

            if (key.StartsWith(RazorUtil.RELATIVE_KEY))
            {
                key = key.Substring(1);
                key = RazorUtil.MakeRelativePath(Key, key);
            }

            return key;
        }

        public virtual HtmlString RenderSectionOrDefault(string name, string defaultName)
        {
            if (IsSectionDefined(name))
                return RenderSection(name);
            else
                return RenderSection(defaultName);
        }

        public virtual HtmlString RenderSectionIfDefined(string name, string layoutKey, object model)
        {
            if (IsSectionDefined(name))
                return RenderSection(name);
            else
                return RenderPart(layoutKey, model);
        }

        protected virtual HtmlString RenderFieldItem(IFieldValueProvider provider, IFieldInfoEx field)
        {
            IRazorField pageData = ViewBag.PageData as IRazorField;
            if (pageData == null)
                return null;

            RazorField razorField = pageData.GetDisplayField(field);
            if (razorField == null)
                return null;
            return razorField.Execute(this, new FieldProviderModelData(provider, field));
        }

        protected virtual HtmlString RenderFieldItem(IFieldValueProvider provider, string tableName, IFieldInfoEx field)
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