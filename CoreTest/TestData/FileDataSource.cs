using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;
using System.IO;
using System.Text;

namespace TestData
{
    [Source(Author = "YJC", CreateDate = "2019/11/13 16:42:59",
        Description = "数据源")]
    internal class FileDataSource : ISource
    {
        public FileDataSource()
        {
        }

        public OutputData DoAction(IInputData input)
        {
            string fileName = Path.Combine(BaseAppSetting.Current.XmlPath, "Application.xml");
            FileData data = FileData.Create(fileName);
            return OutputData.Create(Encoding.UTF8.GetString(data.Data));
        }
    }
}