using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class BaseTemplateItem
    {
        [SimpleAttribute]
        public RazorTemplateStyle Style { get; protected set; }

        [SimpleAttribute]
        public bool IsNormal { get; protected set; }

        [SimpleAttribute]
        public string TemplateFilePath { get; protected set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
