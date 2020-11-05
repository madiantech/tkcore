using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class WebSuccessResult
    {
        protected WebSuccessResult()
        {
        }

        public WebSuccessResult(string message)
        {
            Result = ActionResultData.CreateSuccessResult(message);
        }

        [ObjectElement]
        public ActionResultData Result { get; protected set; }

        [SimpleAttribute(UseSourceType = true)]
        public bool NewWindow { get; set; }

        [SimpleAttribute]
        public string AlertMessage { get; set; }
    }
}
