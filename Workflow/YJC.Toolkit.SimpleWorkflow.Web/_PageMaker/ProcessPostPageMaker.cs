using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ProcessPostPageMaker : BaseObjectPageMaker
    {
        public static readonly IPageMaker DEFAULT = new ProcessPostPageMaker();

        public ProcessPostPageMaker()
        {
            Add(PageMakerUtil.IsType(SourceOutputType.DataSet),
                new JsonDataSetPageMaker());
            Add(PageMakerUtil.IsType(SourceOutputType.ToolkitObject),
                new JsonObjectPageMaker());
            Add(PageMakerUtil.IsType(SourceOutputType.String),
                new SourceOutputPageMaker(ContentTypeConst.JSON));
        }

        protected override IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            //if (outputData.Data is CompleteResult)
            //{
            //    CompleteResult result = (CompleteResult)outputData.getData();
            //    if (result.isEmpty())
            //    {
            //        outputData = OutputData
            //                .createDataNewObject(new WebSuccessResult(WorkflowUtil
            //                        .createTaskListUrl()));
            //    }
            //    else
            //    {
            //        outputData = OutputData
            //                .createDataNewObject(new WebSuccessResult(WorkflowUtil
            //                        .createAutoSignUrl(result.getTaskId(),
            //                                result.getInstanceId())));
            //    }
            //}
            return base.WritePage(source, pageData, outputData);
        }
    }
}