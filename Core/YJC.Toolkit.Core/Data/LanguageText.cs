using System.Collections.Generic;
using System.Globalization;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class LanguageText
    {
        private CultureInfo fCulture;
        private readonly HashSet<CultureInfo> fSet;

        public LanguageText()
        {
            fSet = new HashSet<CultureInfo>();
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
                    fSet.Clear();
                    if (value != null)
                    {
                        do
                        {
                            fSet.Add(value);
                            value = value.Parent;
                        } while (!string.IsNullOrEmpty(value.Name));
                    }
                }
            }
        }

        public bool IsFitFor(CultureInfo info)
        {
            TkDebug.AssertArgumentNull(info, "info", this);

            return fSet.Contains(info);
        }
    }
}
