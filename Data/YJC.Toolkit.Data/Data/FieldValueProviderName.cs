using System;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [TkTypeConverter(typeof(FieldValueProviderNameConverter))]
    internal class FieldValueProviderName
    {
        public FieldValueProviderName(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", null);

            if (name.StartsWith(DecoderConst.DECODER_TAG, StringComparison.CurrentCulture))
            {
                IsDecoder = true;
                SourceName = name.Substring(3);
            }
            else
            {
                IsDecoder = false;
                SourceName = name;
            }
        }

        public bool IsDecoder { get; private set; }

        public string SourceName { get; private set; }

        public string NickName
        {
            get
            {
                return IsDecoder ? SourceName + "_Name" : SourceName;
            }
        }

        public override string ToString()
        {
            return IsDecoder ? DecoderConst.DECODER_TAG + SourceName : SourceName;
        }
    }
}