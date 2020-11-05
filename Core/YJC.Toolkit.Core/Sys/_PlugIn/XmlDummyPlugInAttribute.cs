using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class XmlDummyPlugInAttribute : BasePlugInAttribute
    {
        public XmlDummyPlugInAttribute(string regName, IXmlPlugInItem item)
            : base()
        {
            RegName = regName;
            Author = item.Author;
            CreateDate = item.CreateDate;
            Description = item.Description;
        }

        public override string FactoryName
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}
