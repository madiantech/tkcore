namespace YJC.Toolkit.Sys
{
    internal interface IMultipleElementReader
    {
        bool SupportVersion { get; }

        ObjectPropertyInfo this[QName name] { get; }

        ObjectPropertyInfo this[QName name, string version] { get; }

        ObjectPropertyInfo this[string name] { get; }
    }
}