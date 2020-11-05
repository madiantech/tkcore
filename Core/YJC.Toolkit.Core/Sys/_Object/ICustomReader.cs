namespace YJC.Toolkit.Sys
{
    public interface ICustomReader
    {
        bool SupportVersion { get; }

        CustomPropertyInfo CanRead(string localName, string version);

        object GetValue(string localName, string version);

        void SetValue(string localName, string version, object value);
    }
}