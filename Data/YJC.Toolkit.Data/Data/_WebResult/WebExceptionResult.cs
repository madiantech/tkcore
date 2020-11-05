using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class WebExceptionResult
    {
        public WebExceptionResult(string errorPath)
        {
            TkDebug.AssertArgumentNullOrEmpty(errorPath, "errorPath", null);

            Result = ActionResultData.CreateFailResult(errorPath);
        }

        [ObjectElement]
        public ActionResultData Result { get; private set; }
    }
}
