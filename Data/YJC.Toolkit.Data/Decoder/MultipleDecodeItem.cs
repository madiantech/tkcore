using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class MultipleDecodeItem : IDecoderItem
    {
        private readonly MultipleDecoderData fDecodeData;

        public MultipleDecodeItem(string code, IDecoder decoder, params object[] args)
        {
            fDecodeData = new MultipleDecoderData();
            QuoteStringList list = QuoteStringList.FromString(code);
            var items = list.CreateEnumerable();
            foreach (string item in items)
            {
                IDecoderItem dItem = decoder.Decode(item, args);
                fDecodeData.AddItem(dItem);
            }
            Name = fDecodeData.ToJson();
            DisplayName = Name;
        }

        #region IDecoderItem 成员

        public string Value { get; private set; }

        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public string this[string name]
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}
