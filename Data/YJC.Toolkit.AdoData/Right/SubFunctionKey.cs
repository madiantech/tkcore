using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class SubFunctionKey : IEquatable<SubFunctionKey>
    {
        public SubFunctionKey(IPageStyle style, string source)
        {
            TkDebug.AssertArgumentNull(style, nameof(style), null);
            TkDebug.AssertArgumentNullOrEmpty(source, nameof(source), null);

            Style = style;
            Source = source;
        }

        public IPageStyle Style { get; }

        public string Source { get; }

        public override bool Equals(object obj)
        {
            if (obj is SubFunctionKey other)
                return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            return Style.GetHashCode() ^ Source.GetHashCode();
        }

        public override string ToString() => $"{Style}^{Source}";

        public bool Equals(SubFunctionKey other)
        {
            if (other == null)
                return false;

            return MetaDataUtil.Equals(Style, other.Style) && Source == other.Source;
        }
    }
}