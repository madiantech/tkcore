using System;

namespace YJC.Toolkit.Razor
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RazorTemplateAttribute : Attribute
    {
        public RazorTemplateAttribute(string key, Type templateType)
        {
            Key = key;
            TemplateType = templateType;
        }

        public string Key { get; }

        public Type TemplateType { get; }
    }
}