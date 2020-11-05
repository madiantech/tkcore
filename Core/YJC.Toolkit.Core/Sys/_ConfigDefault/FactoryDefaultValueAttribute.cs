using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class FactoryDefaultValueAttribute : BaseDefaultValueAttribute
    {
        public FactoryDefaultValueAttribute(string sectionName, string configFactoryName)
        {
            TkDebug.AssertArgumentNullOrEmpty(sectionName, nameof(sectionName), null);
            TkDebug.AssertArgumentNullOrEmpty(configFactoryName, nameof(configFactoryName), null);

            SectionName = sectionName;
            ConfigFactoryName = configFactoryName;
        }

        public string SectionName { get; }

        public string ConfigFactoryName { get; }
    }
}