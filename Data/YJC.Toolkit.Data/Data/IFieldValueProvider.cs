using System.Collections.Generic;
using YJC.Toolkit.Decoder;

namespace YJC.Toolkit.Data
{
    public interface IFieldValueProvider
    {
        object this[string nickName] { get; }

        IEnumerable<IDecoderItem> GetCodeTable(string regName);

        bool IsEmpty { get; }
    }
}
