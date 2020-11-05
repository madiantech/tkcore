namespace YJC.Toolkit.Decoder
{
    public interface IDecoder
    {
        IDecoderItem Decode(string code, params object[] args);

        void Fill(params object[] args);
    }
}
