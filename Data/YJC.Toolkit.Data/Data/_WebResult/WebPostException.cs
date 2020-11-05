using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Serializable]
    public class WebPostException : Exception
    {
        private readonly List<FieldErrorInfo> fErrorFields;

        public WebPostException(string message)
            : base(message)
        {
        }

        public WebPostException(string message, FieldErrorInfo errorField)
            : this(message, EnumUtil.Convert(errorField))
        {
        }

        public WebPostException(string message, IEnumerable<FieldErrorInfo> errorFields)
            : this(message)
        {
            fErrorFields = new List<FieldErrorInfo>();
            fErrorFields.AddRange(errorFields);
        }

        public WebErrorResult CreateErrorResult()
        {
            WebErrorResult result = new WebErrorResult(Message);
            if (fErrorFields != null)
                foreach (var item in fErrorFields)
                    result.Add(item);

            return result;
        }
    }
}
