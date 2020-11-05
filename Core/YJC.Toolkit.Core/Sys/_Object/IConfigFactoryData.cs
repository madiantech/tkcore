using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.Sys
{
    internal interface IConfigFactoryData
    {
        bool SupportVersion { get; }

        ObjectElementAttribute this[string name] { get; }

        ObjectElementAttribute this[Type type] { get; }

        ObjectElementAttribute GetObjectElementAttribute(QName name, string version);

        ObjectElementAttribute GetObjectElementAttribute(string name, string version);

        void Add(BaseXmlConfigFactory factory, string regName,
            BaseObjectElementAttribute attr, Type type);
    }
}