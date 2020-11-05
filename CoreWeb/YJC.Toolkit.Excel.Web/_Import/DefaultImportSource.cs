using System.Dynamic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    internal class DefaultImportSource : ISource
    {
        private readonly ImportConfigXml fConfig;
        private readonly dynamic fBag;

        public DefaultImportSource(ImportConfigXml config)
        {
            fConfig = config;
            fBag = new ExpandoObject();
            fBag.Title = fConfig.Import.Title.ToString();
        }

        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            input.CallerInfo.AddInfo(fBag);
            return OutputData.CreateObject(fBag);
        }

        #endregion ISource 成员
    }
}