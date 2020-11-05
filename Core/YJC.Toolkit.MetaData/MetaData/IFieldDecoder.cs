using System.Collections.Generic;

namespace YJC.Toolkit.MetaData
{
    public interface IFieldDecoder
    {
        DecoderType Type { get; }

        string RegName { get; }

        IEnumerable<DecoderAdditionInfo> Additions { get; }
    }
}
