using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Web
{
    internal static class WeChatWebUtil
    {
        public static void WriteRawFile(IInputData input, MemoryStream dataStream)
        {
            string path = Path.Combine(BaseAppSetting.Current.XmlPath, @"Weixin\Raw");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileName = Path.Combine(path, string.Format(ObjectUtil.SysCulture,
                "wei{0:HHmmssffff}.txt", DateTime.Now));

            using (FileStream file = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
            {
                byte[] data;
                IPageData pageData = input as IPageData;
                if (pageData != null)
                {
                    data = ToolkitConst.UTF8.GetBytes(pageData.PageUrl.ToString());
                    file.Write(data, 0, data.Length);
                }
                data = dataStream.ToArray();
                file.Write(data, 0, data.Length);
                file.Flush();
            }
        }
    }
}