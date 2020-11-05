namespace YJC.Toolkit.Sys
{
    public interface ITkTypeConverter
    {
        string DefaultValue { get; }

        object ConvertFromString(string text, ReadSettings settings);

        string ConvertToString(object value, WriteSettings settings);
    }
}
