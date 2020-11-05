using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public sealed class DecoderAdditionInfo
    {
        internal DecoderAdditionInfo()
        {
        }

        public DecoderAdditionInfo(string decoderNickName, string dataNickName)
        {
            DecoderNickName = decoderNickName;
            DataNickName = dataNickName;
        }

        [SimpleAttribute(Required = true)]
        public string DecoderNickName { get; private set; }

        [SimpleAttribute(Required = true)]
        public string DataNickName { get; private set; }
    }
}
