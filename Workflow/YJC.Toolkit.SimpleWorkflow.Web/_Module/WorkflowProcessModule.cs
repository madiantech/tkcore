using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Log;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowProcessModule : IModule, ISupportLog
    {
        private readonly ProcessXml fProcessXml;

        public WorkflowProcessModule(ProcessXml processXml)
        {
            fProcessXml = processXml;
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
            return new WorkflowMetaData(pageData, fProcessXml.TableList);
        }

        public ISource CreateSource(IPageData pageData)
        {
            if (pageData.IsPost)
                return new WfProcessSource(fProcessXml);
            else
                return new WfProcessDetailSource(fProcessXml);
        }

        public IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            return JsonPostDataSetObjectCreator.Creator;
        }

        public IPageMaker CreatePageMaker(IPageData pageData)
        {
            if (pageData.IsPost)
                return new JsonObjectPageMaker();
            else
            {
                if (WorkflowContentModule.IsShowSource(true, pageData))
                    return XmlPageMaker.DEFAULT;
                if (WorkflowContentModule.IsShowMetaData(true, pageData))
                    return WorkflowContentModule.CreatePageMaker("<tk:MetaDataPageMaker />", pageData);
                RazorPageTemplatePageMaker pageMaker = new RazorPageTemplatePageMaker("WfProcess", pageData);
                if (fProcessXml.PageMaker != null)
                {
                    var pmConfig = fProcessXml.PageMaker;
                    if (!string.IsNullOrEmpty(pmConfig.RazorFile))
                        pageMaker.RazorFile = pmConfig.RazorFile;
                    if (pmConfig.PageData != null)
                        pageMaker.PageData = pmConfig.PageData.CreateObject();
                    if (pmConfig.Scripts != null && pmConfig.Scripts.Count > 0)
                        pageMaker.Scripts = new UserScript(pmConfig.Scripts);
                }
                return pageMaker;
            }
        }

        public IRedirector CreateRedirector(IPageData pageData)
        {
            throw new NotSupportedException();
        }

        public bool IsSupportLogOn(IPageData pageData)
        {
            return true;
        }

        public bool IsDisableInjectCheck(IPageData pageData)
        {
            return false;
        }

        public bool IsCheckSubmit(IPageData pageData)
        {
            return true;
        }

        #endregion IModule 成员

        public void PrepareRecordLog(IInputData input, ISource source)
        {
            if (fProcessXml.RecordLogs == null)
                return;
            ISupportRecordLog recordLog = source as ISupportRecordLog;
            if (recordLog == null)
                return;

            foreach (var item in fProcessXml.RecordLogs)
            {
                var dataPicker = item.DataPicker.CreateObject(input);
                recordLog.SetRecordDataPicker(item.TableName, dataPicker);
            }
        }

        public void Log(IInputData input, ISource source, OutputData output)
        {
            //var log = fProcessXml.GetLog(source, input, output);
            //if (log != null)
            //{
            //    ILogData data = log.PickLogData(input, source, output);
            //    log.Log(data);
            //}
            if (fProcessXml.RecordLogs != null)
            {
                ISupportRecordLog recordLog = source as ISupportRecordLog;
                if (recordLog != null)
                    foreach (var item in fProcessXml.RecordLogs)
                    {
                        ILog logData = item.LogData.CreateObject();
                        var data = recordLog.GetRecordDatas(item.TableName);
                        BaseGlobalVariable.Current.BeginInvoke(
                            new Action<ILog, IEnumerable<ILogData>>(Log), logData, data);
                        //logData.LogData(data);
                    }
            }
        }

        private static void Log(ILog log, IEnumerable<ILogData> data)
        {
            try
            {
                log.LogData(data);
            }
            catch (Exception ex)
            {
                TkTrace.LogError(ex.Message);
            }
        }
    }
}