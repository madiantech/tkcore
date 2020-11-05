using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace YJC.Toolkit.Sys
{
    public class PathStringParser
    {
        private readonly string[] fParts;
        private readonly string[] fOtherParts;

        public PathStringParser(PathString path)
        {
            string value = path.Value;
            if ((bool)value?.StartsWith('/'))
                value = value.Substring(1);
            fParts = value.Split('/', StringSplitOptions.None);
            if (Count > 0)
            {
                Parser = fParts[0]?.ToLower();
                fOtherParts = new string[Count - 1];
                Array.Copy(fParts, 1, fOtherParts, 0, fOtherParts.Length);
            }
        }

        public int Count
        {
            get
            {
                return fParts.Length;
            }
        }

        public string Parser { get; }

        public void Parse(Action<string[]> action)
        {
            action?.Invoke(fOtherParts);
        }
    }
}