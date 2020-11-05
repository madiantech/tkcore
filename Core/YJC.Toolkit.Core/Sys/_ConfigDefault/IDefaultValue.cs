using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public interface IDefaultValue
    {
        Dictionary<string, object> GetFactorySection(string sectionName);

        object GetFactoryDefaultObject(string sectionName, string objectName);

        object GetDefaultObject(string objectName);

        string GetSimpleDefaultValue(string keyName);

        void RegisterConfig(DefaultValueTypeFactory factory);
    }
}