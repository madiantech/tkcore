
namespace YJC.Toolkit.Sys
{
    public sealed class DictionaryOutput
    {
        public static readonly DictionaryOutput Default = new DictionaryOutput { IgnoreEmpty = true };

        public bool IgnoreEmpty { get; private set; }
    }
}
