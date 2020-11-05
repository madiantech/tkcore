using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using YJC.Toolkit.Log;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    public sealed class Tk5ModuleXml : ToolkitConfig, IModule, ISupportLog, IFileDependency
    {
        private const string EXCEL_PAGEMAKER = "<tk:ExportExcelPageMaker />";
        private const string JSON_PAGEMAKER = "<tk:JsonPageMaker />";

        public Tk5ModuleXml()
        {
            ExpectedVersion = "5.0";
        }

        #region IModule 成员

        public string Title
        {
            get
            {
                return Module.GetTitle();
            }
        }

        public IMetaData CreateMetaData(IPageData pageData)
        {
            if (Module.MetaData == null)
                return null;
            IMetaData metaData = Module.MetaData.CreateObject(pageData);
            if (metaData != null)
                metaData.Title = Module.GetTitle();
            return metaData;
        }

        public ISource CreateSource(IPageData pageData)
        {
            //TkDebug.AssertNotNull(Module.Source, "", this);
            return Module.Source.CreateObject(pageData);
        }

        public IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            if (Module.PostObjectCreator != null)
                return Module.PostObjectCreator.CreateObject(pageData);
            else
                return WebAppSetting.WebCurrent.DefaultPostCreator.CreateObject(pageData);
        }

        public IPageMaker CreatePageMaker(IPageData pageData)
        {
            if (PageMakerUtil.IsShowSource(Module.ShowSource, pageData))
                return PageMakerUtil.XmlPageMaker;
            if (PageMakerUtil.IsShowMetaData(Module.ShowSource, pageData))
                return new MetaDataPageMaker();
            if (PageMakerUtil.IsShowJson(Module.ShowSource, pageData))
                return JSON_PAGEMAKER.CreateFromXmlFactory<IPageMaker>(PageMakerConfigFactory.REG_NAME);
            if (PageMakerUtil.IsShowExcel(Module.ShowSource, pageData))
            {
                var pageMaker = EXCEL_PAGEMAKER.ReadXmlFromFactory<IConfigCreator<IPageMaker>>(
                    PageMakerConfigFactory.REG_NAME);
                if (pageMaker != null)
                    return pageMaker.CreateObject(pageData);
            }

            if (Module.PageMaker != null)
                return Module.PageMaker.CreateObject(pageData);
            else
                return WebAppSetting.WebCurrent.DefaultPageMaker.CreateObject(pageData);
        }

        public IRedirector CreateRedirector(IPageData pageData)
        {
            if (Module.Redirector != null)
                return Module.Redirector.CreateObject(pageData);
            else
                return WebAppSetting.WebCurrent.DefaultRedirector.CreateObject(pageData);
        }

        public bool IsSupportLogOn(IPageData pageData)
        {
            return Module.IsSupportLogon(pageData);
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

        #region ISupportLog 成员

        public void PrepareRecordLog(IInputData input, ISource source)
        {
            if (Module.RecordLogs == null)
                return;
            ISupportRecordLog recordLog = source as ISupportRecordLog;
            if (recordLog == null)
                return;

            foreach (var item in Module.RecordLogs)
            {
                var dataPicker = item.DataPicker.CreateObject(input);
                recordLog.SetRecordDataPicker(item.TableName, dataPicker);
            }
        }

        public void Log(IInputData input, ISource source, OutputData output)
        {
            var log = Module.GetLog(source, input, output);
            if (log != null)
            {
                ILogData data = log.PickLogData(input, source, output);
                log.Log(data);
            }
            if (Module.RecordLogs != null)
            {
                ISupportRecordLog recordLog = source as ISupportRecordLog;
                if (recordLog != null)
                    foreach (var item in Module.RecordLogs)
                    {
                        ILog logData = item.LogData.CreateObject();
                        var data = recordLog.GetRecordDatas(item.TableName);
                        BaseGlobalVariable.Current.BeginInvoke(
                            new Action<ILog, IEnumerable<ILogData>>(Log), logData, data);
                        //logData.LogData(data);
                    }
            }
        }

        #endregion ISupportLog 成员

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        internal Tk5ModuleConfigItem Module { get; private set; }

        public IEnumerable<string> Files { get; private set; }

        protected override void OnSetFullPath(string path)
        {
            Files = EnumUtil.Convert(path);
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