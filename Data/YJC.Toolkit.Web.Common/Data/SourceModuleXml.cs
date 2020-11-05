using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    internal sealed class SourceModuleXml : IModule
    {
        private readonly ISource fSource;
        private readonly BasePostObjectCreatorAttribute fPostObjectAttr;
        private readonly BasePageMakerAttribute fPageMakerAttr;
        private readonly BaseRedirectorAttrbiute fRedirectorAttr;
        private readonly WebPageAttribute fPageAttr;

        public SourceModuleXml(ISource source)
        {
            fSource = source;
            Type type = fSource.GetType();
            fPostObjectAttr = GetType<BasePostObjectCreatorAttribute>(type);
            fPageMakerAttr = GetType<BasePageMakerAttribute>(type);
            fRedirectorAttr = GetType<BaseRedirectorAttrbiute>(type);
            fPageAttr = GetType<WebPageAttribute>(type);
        }

        #region IModule 成员

        public string Title
        {
            get
            {
                return string.Empty;
            }
        }

        public IMetaData CreateMetaData(IPageData pageData)
        {
            return null;
        }

        public ISource CreateSource(IPageData pageData)
        {
            return fSource;
        }

        public IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            if (fPostObjectAttr != null)
                return fPostObjectAttr.CreatePostObjectCreator(pageData);
            else
                return WebAppSetting.WebCurrent.DefaultPostCreator.CreateObject(pageData);
        }

        public IPageMaker CreatePageMaker(IPageData pageData)
        {
            if (PageMakerUtil.IsShowSource(true, pageData))
                return PageMakerUtil.XmlPageMaker;

            if (fPageMakerAttr != null)
                return fPageMakerAttr.CreatePageMaker(pageData);
            else
                return WebAppSetting.WebCurrent.DefaultPageMaker.CreateObject(pageData);
        }

        public IRedirector CreateRedirector(IPageData pageData)
        {
            if (fRedirectorAttr != null)
                return fRedirectorAttr.CreateRedirector(pageData);
            else
                return WebAppSetting.WebCurrent.DefaultRedirector.CreateObject(pageData);
        }

        public bool IsSupportLogOn(IPageData pageData)
        {
            if (fPageAttr != null)
                return fPageAttr.SupportLogOn;
            return true;
        }

        public bool IsDisableInjectCheck(IPageData pageData)
        {
            if (fPageAttr != null)
                return fPageAttr.DisableInjectCheck;
            return false;
        }

        public bool IsCheckSubmit(IPageData pageData)
        {
            if (fPageAttr != null)
                return fPageAttr.CheckSubmit;
            return true;
        }

        #endregion IModule 成员

        private static T GetType<T>(Type objType) where T : Attribute
        {
            Attribute attr = Attribute.GetCustomAttribute(objType, typeof(T));
            if (attr != null)
                return attr as T;
            return null;
        }
    }
}