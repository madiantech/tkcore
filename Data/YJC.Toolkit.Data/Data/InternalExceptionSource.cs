using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Source(Author = "YJC", CreateDate = "2015-08-16", RegName = "ShowException",
        Description = "显示Error目录下对应错误文件的详细信息")]
    internal class InternalExceptionSource : ExceptionSource
    {
        public override OutputData DoAction(IInputData input)
        {
            string fileName = input.QueryString["FileName"];
            fileName = Path.Combine(BaseAppSetting.Current.ErrorPath, fileName + ".xml");
            ExceptionData data = new ExceptionData();
            data.ReadXmlFromFile(fileName);
            Data = data;

            return base.DoAction(input);
        }
    }
}
