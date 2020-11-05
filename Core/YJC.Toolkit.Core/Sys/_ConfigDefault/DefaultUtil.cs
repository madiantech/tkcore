using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    public static class DefaultUtil
    {
        public static string GetSimpleValue(string keyName, string defaultValue)
        {
            TkDebug.AssertArgumentNullOrEmpty(keyName, nameof(keyName), null);
            TkDebug.ThrowIfNoGlobalVariable();

            string configValue = BaseGlobalVariable.Current.DefaultValue.GetSimpleDefaultValue(keyName);
            return configValue ?? defaultValue;
        }

        public static object GetFactoryObject(string sectionName, string objectName)
        {
            TkDebug.AssertArgumentNullOrEmpty(sectionName, nameof(sectionName), null);
            TkDebug.AssertArgumentNullOrEmpty(objectName, nameof(objectName), null);
            TkDebug.ThrowIfNoGlobalVariable();

            object result = BaseGlobalVariable.Current.DefaultValue.GetFactoryDefaultObject(
                sectionName, objectName);
            return result;
        }

        public static bool CreateConfigObject<T>(object creatorObject, out T obj)
        {
            if (creatorObject is IConfigCreator<T> creator)
            {
                obj = creator.CreateObject();
                return true;
            }
            obj = default;
            return false;
        }
    }
}