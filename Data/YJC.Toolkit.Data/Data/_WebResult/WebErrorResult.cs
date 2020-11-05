using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class WebErrorResult
    {
        private List<FieldErrorInfo> fFieldInfos;

        protected WebErrorResult()
        {
        }

        public WebErrorResult(string message)
        {
            Result = ActionResultData.CreateErrorResult(message);
        }

        [ObjectElement]
        public ActionResultData Result { get; private set; }

        [ObjectElement(IsMultiple = true, CollectionType = typeof(List<FieldErrorInfo>),
            ObjectType = typeof(FieldErrorInfo), LocalName = "FieldInfo")]
        public IEnumerable<FieldErrorInfo> FieldInfos
        {
            get
            {
                return fFieldInfos;
            }
            protected set
            {
                fFieldInfos = value as List<FieldErrorInfo>;
            }
        }

        public void Add(FieldErrorInfo errorInfo)
        {
            TkDebug.AssertArgumentNull(errorInfo, "errorInfo", this);

            if (fFieldInfos == null)
                fFieldInfos = new List<FieldErrorInfo>();
            fFieldInfos.Add(errorInfo);
        }
    }
}
