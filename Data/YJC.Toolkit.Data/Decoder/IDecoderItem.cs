namespace YJC.Toolkit.Decoder
{
    public interface IDecoderItem
    {
        string Value { get; }

        string Name { get; }

        string DisplayName { get; }

        string this[string name] { get; }
    }
}
