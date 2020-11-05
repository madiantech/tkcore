using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-08-17",
        Description = @"使用BootCss\Bin\Exception.cshtml输出Get的错误，发送文件Id输出Post的错误的PageMaker(Exception专用)")]
    internal class ExceptionRazorPageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.QueryObject<IInputData>(args);
            TkDebug.AssertNotNull(input, "参数args中缺少IInputData类型", this);

            if (input.IsPost)
                return new ExceptionPostPageMaker();
            else
            {
                string fileName = Path.Combine(BaseAppSetting.Current.XmlPath,
                    @"razortemplate\BootCss\Bin\Exception.cshtml");
                return new FreeRazorPageMaker(fileName);
            }
        }

        #endregion
    }
}
