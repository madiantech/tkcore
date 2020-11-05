using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2016-07-04", 
        Description = "")]
    internal class RazorStylePageMakerConfig : BaseRazorPageMakerConfig, IConfigCreator<IPageMaker>
    {
        public RazorStylePageMakerConfig()
        {
        }

        [SimpleAttribute]
        public RazorTemplateStyle TemplateStyle { get; private set; }

        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            IPageData pageData = ObjectUtil.ConfirmQueryObject<IPageData>(this, args);

            return new RazorStylePageMaker(this, pageData);
        }

        #endregion
    }
}
