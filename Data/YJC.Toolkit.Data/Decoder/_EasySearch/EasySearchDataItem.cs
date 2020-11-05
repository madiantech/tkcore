using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class EasySearchDataItem<T> : IRegName where T : IDecoderItem, IRegName
    {
        private readonly T fItem;

        public EasySearchDataItem(T item)
        {
            TkDebug.AssertArgumentNull(item, "item", null);

            fItem = item;
            Pinyin = PinYinUtil.GetPyHeader(Item.Name, string.Empty);
        }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return fItem.RegName;
            }
        }

        #endregion

        public T Item
        {
            get
            {
                return fItem;
            }
        }

        public string Pinyin { get; private set; }
    }
}
