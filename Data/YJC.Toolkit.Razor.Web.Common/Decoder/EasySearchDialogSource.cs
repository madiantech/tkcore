using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Decoder
{
    [FreeRazorPageMaker("EasySearch/DialogTemplate.cshtml", UseTemplate = true)]
    [Source(Author = "YJC", CreateDate = "2014-08-21", Description = "普通EasySearch对话框外壳")]
    [WebPage(SupportLogOn = false)]
    internal class EasySearchDialogSource : DynamicObjectSource
    {
        public EasySearchDialogSource()
        {
            UseCallerInfo = true;
        }

        protected override void AddObject(IInputData input, dynamic bag)
        {
        }
    }
}