using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Decoder
{
    [FreeRazorPageMaker("EasySearch/TreeTemplate.cshtml", UseTemplate = true)]
    [Source(Author = "YJC", CreateDate = "2014-08-21", Description = "树形EasySearch对话框外壳")]
    [WebPage(SupportLogOn = false)]
    internal class EasySearchTreeDialogSource : DynamicObjectSource
    {
        public EasySearchTreeDialogSource()
        {
            UseCallerInfo = true;
        }

        protected override void AddObject(IInputData input, dynamic bag)
        {
        }
    }
}