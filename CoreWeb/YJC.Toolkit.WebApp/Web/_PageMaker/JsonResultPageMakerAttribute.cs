using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class JsonResultPageMakerAttribute : BasePageMakerAttribute, IObjectFormat
    {
        public JsonResultPageMakerAttribute(ActionResult result)
        {
            Result = result;
        }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new JsonResultPageMaker(this);
        }

        public ActionResult Result { get; private set; }

        public string Message { get; set; }

        public ConfigType GZip { get; set; }

        public ConfigType Encrypt { get; set; }
    }
}
