using System.Collections.Generic;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ObjectDecoderContainer
    {
        private readonly Dictionary<string, IDecoderItem> fDictionary;

        public ObjectDecoderContainer()
        {
            fDictionary = new Dictionary<string, IDecoderItem>();
        }

        public void Add(string nickName, IDecoderItem item)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);

            if (item == null)
                return;

            fDictionary[nickName] = item;
        }

        public IDecoderItem this[string nickName]
        {
            get
            {
                IDecoderItem result = ObjectUtil.TryGetValue(fDictionary, nickName);
                return result;
            }
        }

        public string GetNameString(string nickName)
        {
            var item = this[nickName];
            return item == null ? string.Empty : item.Name;
        }
    }
}
