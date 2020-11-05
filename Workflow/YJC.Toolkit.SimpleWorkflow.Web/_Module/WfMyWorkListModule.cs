using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Module(Author = "YJC", CreateDate = "2017-10-03", Description = "我的工作-子列表")]
    internal class WfMyWorkListModule : BaseCustomModule
    {
        public WfMyWorkListModule()
            : base("子列表")
        {
        }

        public override ISource CreateSource(IPageData pageData)
        {
            return new WfMyWorkListSource();
        }

        public override IMetaData CreateMetaData(IPageData pageData)
        {
            const string xml = "<tk:SingleXmlMetaData DataXml=\"Workflow/WorkflowInst.xml\"/>";
            var metaData = xml.ReadXmlFromFactory<IConfigCreator<IMetaData>>(
                MetaDataConfigFactory.REG_NAME);
            return metaData.CreateObject(pageData);
        }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            const string xml = "<tk:SingleRazorPageMaker/>";
            var maker = xml.ReadXmlFromFactory<IConfigCreator<IPageMaker>>(
                PageMakerConfigFactory.REG_NAME);
            return maker.CreateObject(pageData);
        }
    }
}