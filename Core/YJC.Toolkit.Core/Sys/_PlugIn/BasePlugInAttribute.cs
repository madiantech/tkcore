using System;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class BasePlugInAttribute : Attribute, IRegName, IAuthor
    {
        protected BasePlugInAttribute()
        {
        }

        #region IRegName 成员

        public virtual string RegName { get; set; }

        #endregion

        #region IAuthor 成员

        public string Author { get; set; }

        public string Description { get; set; }

        public string CreateDate { get; set; }

        #endregion

        public abstract string FactoryName { get; }

        public string Suffix { get; protected set; }

        protected virtual string CalRegName(string typeName)
        {
            if (!string.IsNullOrEmpty(Suffix) && typeName.EndsWith(Suffix, StringComparison.Ordinal))
            {
                typeName = typeName.Substring(0, typeName.Length - Suffix.Length);
            }
            return typeName;
        }

        public string GetRegName(MemberInfo type)
        {
            TkDebug.AssertArgumentNull(type, "type", this);

            if (string.IsNullOrEmpty(RegName))
            {
                string typeName = type.Name;
                return CalRegName(typeName);
            }
            else
                return RegName;
        }
    }
}
