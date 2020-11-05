using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Decoder
{
    [FreeRazorPageMaker("razortemplate/bootcss/easysearch/dialogtemplate.cshtml")]
    [Source(Author = "YJC", CreateDate = "2014-08-21", Description = "普通EasySearch对话框外壳")]
    [WebPage(SupportLogOn = false)]
    class EasySearchDialogSource : DynamicObjectSource
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
