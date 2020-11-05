using System;
using System.Text;
using System.Threading.Tasks;
using YJC.Toolkit.Data;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public static class ExceptionUtil
    {
        private static void SaveFile(string fileName, string content, Encoding encoding)
        {
            FileUtil.VerifySaveFile(fileName, content, encoding);
        }

        public static string LogException(ExceptionData logData)
        {
            if (logData == null)
                return null;

            TkDebug.ThrowIfNoAppSetting();
            if (!BaseAppSetting.Current.UseWorkThread)
                return null;

            TkDebug.ThrowIfNoGlobalVariable();
            string fileName = WebAppSetting.WebCurrent.GetExceptionLogName(logData.Exception);
            string content = logData.WriteXml();
            Encoding encoding = BaseAppSetting.Current.WriteSettings.Encoding;
            BaseGlobalVariable.Current.BeginInvoke(new Action<string, string, Encoding>(SaveFile),
                fileName, content, encoding);

            return fileName;
        }

        internal static void HandleStartExecption(string startName, Type errorType, Exception ex)
        {
            string type = errorType != null ? errorType.ToString() : string.Empty;
            ExceptionData exData = new ExceptionData(null, type, null, ex);

            TkDebug.ThrowIfNoAppSetting();
            if (!BaseAppSetting.Current.UseWorkThread)
                return;

            TkDebug.ThrowIfNoGlobalVariable();
            string fileName = WebAppSetting.WebCurrent.GetStartLogName(startName, exData.Exception);
            string content = exData.WriteXml();
            Encoding encoding = WriteSettings.Default.Encoding;
            SaveFile(fileName, content, encoding);
            //BaseGlobalVariable.Current.BeginInvoke(new Action<string, string, Encoding>(SaveFile),
            //    fileName, content, encoding);
        }
    }
}