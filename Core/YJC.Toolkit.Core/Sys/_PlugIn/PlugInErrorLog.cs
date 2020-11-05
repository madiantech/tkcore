using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public sealed class PlugInErrorLog
    {
        internal PlugInErrorLog()
        {
            Code = new List<ErrorPlugInInfo>();
            Xml = new List<ErrorPlugInInfo>();
        }

        public bool HasError
        {
            get
            {
                return Code.Count > 0 || Xml.Count > 0;
            }
        }

        [TagElement]
        [ObjectElement(IsMultiple = true, LocalName = "Item")]
        public List<ErrorPlugInInfo> Code { get; private set; }

        [TagElement]
        [ObjectElement(IsMultiple = true, LocalName = "Item")]
        public List<ErrorPlugInInfo> Xml { get; private set; }

        internal void AddCodeError(BasePlugInAttribute attribute, Type type, PlugInErrorType errorType)
        {
            Code.Add(new ErrorPlugInInfo(attribute, type, errorType));
        }

        internal void AddXmlError(string regName, string xmlFile, PlugInErrorType errorType)
        {
            Xml.Add(new ErrorPlugInInfo(regName, xmlFile, errorType));
        }
    }
}
