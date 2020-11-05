using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    [JsonObjectPageMaker]
    [Source(Author = "YJC", CreateDate = "2018-04-09", Description = "NonUI操作按钮页面")]
    internal class WfOperationProcessSource : BaseDbSource
    {
        private Operation fOperation;
        private Workflow fWorkflow;
        private WorkflowContent fContent;

        public WfOperationProcessSource()
        {
            Context = WorkflowContext.CreateDbContext();
        }

        private void CreateProcessor(IInputData input)
        {
            string workflowId = input.QueryString[WorkflowWebConst.QUERY_STRING_ID];
            string regName = input.QueryString["RegName"];
            TkDebug.AssertNotNullOrEmpty(workflowId, "QueryString的参数WFID 不能为空", this);
            TkDebug.AssertNotNullOrEmpty(regName, "QueryString的参数PlugIn 不能为空", this);

            fWorkflow = Workflow.CreateWorkflow(workflowId, this);
            fContent = WorkflowInstResolver.CreateContent(fWorkflow.WorkflowRow);

            fOperation = PlugInFactoryManager.CreateInstance<Operation>(
                OperationPlugInFactory.REG_NAME, regName);
            fOperation.Config = fWorkflow.CurrentStep.Config;
            fOperation.Content = fContent;
            fOperation.Source = this;
        }

        private WebSuccessResult ExecuteOperation(IParameter parameter)
        {
            WebSuccessResult result = new WebSuccessResult(WebUtil.ResolveUrl(WorkflowWebConst.MYWORK_URL));
            if (fOperation.Execute(fWorkflow, parameter, WebGlobalVariable.UserId))
            {
                string message;
                switch (fOperation.Action)
                {
                    case StepAction.Unlock:
                    case StepAction.Abort:
                        return result;

                    case StepAction.None:
                        message = string.IsNullOrEmpty(fOperation.Message) ? "操作成功" : fOperation.Message;
                        result.AlertMessage = message;
                        return result;

                    default:
                        return new WebSuccessResult(WebUtil.ResolveUrl(string.Format(ObjectUtil.SysCulture,
                            WorkflowWebConst.STATUS_URL, fWorkflow.WorkflowId)));
                }
            }
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fWorkflow.DisposeObject();
                fContent.DisposeObject();
                fOperation.DisposeObject();
            }

            base.Dispose(disposing);
        }

        public override OutputData DoAction(IInputData input)
        {
            CreateProcessor(input);
            IParameter parameter = Parameter.Create(input.QueryString);
            try
            {
                return OutputData.CreateToolkitObject(ExecuteOperation(parameter));
            }
            catch (Exception ex)
            {
                return OutputData.CreateToolkitObject(new WebErrorResult(ex.Message));
            }
        }
    }
}