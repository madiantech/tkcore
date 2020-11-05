using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Module(Author = "YJC", CreateDate = "2017-07-09", Description = "工作流-我的工作")]
    public class WfMyWorkModule : BaseCustomModule
    {
        public WfMyWorkModule()
            : base("我的工作")
        {
        }

        public override ISource CreateSource(IPageData pageData)
        {
            return new WfMyWorkSource();
        }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            const string xml = "<tk:FreeRazorPageMaker FileName=\"Workflow/MyWork.cshtml\"/>";
            var maker = xml.ReadXmlFromFactory<IConfigCreator<IPageMaker>>(
                PageMakerConfigFactory.REG_NAME);
            return maker.CreateObject(pageData);
            //return XmlPageMaker.DEFAULT;
        }
    }
}