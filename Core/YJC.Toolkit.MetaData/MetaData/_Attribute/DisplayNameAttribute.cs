using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public sealed class DisplayNameAttribute : Attribute
    {
        public DisplayNameAttribute(string displayName)
        {
            TkDebug.AssertArgumentNullOrEmpty(displayName, "displayName", null);

            DisplayName = displayName;
        }

        public string DisplayName { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            DisplayNameAttribute attr = obj as DisplayNameAttribute;
            return attr != null && attr.DisplayName == DisplayName;
        }

        public override int GetHashCode()
        {
            return DisplayName.GetHashCode();
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
