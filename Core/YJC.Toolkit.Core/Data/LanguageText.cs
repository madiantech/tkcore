using System.Collections.Generic;
using System.Globalization;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class LanguageText
    {
        private CultureInfo fCulture;

        public LanguageText()
        {
        }

        [TextContent]
        public string Value { get; private set; }

        [SimpleAttribute]
        public CultureInfo Culture
        {
            get
            {
                return fCulture;
            }
            private set
            {
                if (fCulture != value)
                {
                    fCulture = value;
                }
            }
        }

        public bool IsFitFor(CultureInfo info)
        {
            TkDebug.AssertArgumentNull(info, "info", this);

            while (!string.IsNullOrEmpty(info.Name))
            {
                if (info.Name == fCulture.Name)
                    return true;

                info = info.Parent;
            }
            return false;
        }
    }
}