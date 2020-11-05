using System;

namespace YJC.Toolkit.Sys
{
    public sealed class ErrorPlugInInfo
    {
        internal ErrorPlugInInfo(BasePlugInAttribute attribute, Type type, PlugInErrorType errorType)
        {
            RegName = attribute.GetRegName(type);
            Type = type.ToString();
            AssemblyName = type.Assembly.FullName;
            Error = errorType;
        }

        internal ErrorPlugInInfo(string regName, string xmlFile, PlugInErrorType errorType)
        {
            RegName = regName;
            AssemblyName = xmlFile;
            Error = errorType;
        }

        [SimpleElement]
        public string RegName { get; private set; }

        [SimpleElement]
        public string Type { get; private set; }

        [SimpleElement]
        public string AssemblyName { get; private set; }

        [SimpleElement]
        public PlugInErrorType Error { get; private set; }
    }
}
