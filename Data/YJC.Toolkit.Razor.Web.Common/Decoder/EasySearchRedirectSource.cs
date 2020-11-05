using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Decoder
{
    [WebPage(SupportLogOn = false)]
    [Source(Author = "YJC", CreateDate = "2014-08-21",
        Description = "检测EasySearch的类型并决定跳转到树形EasySearch或者普通EasySearch对话框的界面上")]
    [OutputRedirector]
    internal class EasySearchRedirectSource : ISource
    {
        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            string regName = input.QueryString["RegName"];
            EasySearch easySearch = PlugInFactoryManager.CreateInstance<EasySearch>(
                EasySearchPlugInFactory.REG_NAME, regName);
            IConfigCreator<ITree> creator = easySearch as IConfigCreator<ITree>;
            string url;
            if (creator != null)
                url = string.Format(ObjectUtil.SysCulture,
                    "~/c/source/C/EasySearchTreeDialog?RegName={0}&InitValue={1}",
                    regName, input.QueryString["InitValue"]);
            else
                url = string.Format(ObjectUtil.SysCulture,
                    "~/c/source/C/EasySearchDialog?RegName={0}&RefValue={1}&Addition={2}",
                    regName, input.QueryString["RefValue"], input.QueryString["Addition"]);

            return OutputData.Create(url);
        }

        #endregion ISource 成员
    }
}