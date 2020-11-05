using System;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SourceWebPageAttribute : Attribute
    {
        public SourceWebPageAttribute()
        {
            CheckSubmit = true;
            SupportLogOn = true;
        }

        public bool SupportLogOn { get; set; }

        public bool CheckSubmit { get; set; }

        public bool DisableInjectCheck { get; set; }
    }
}
