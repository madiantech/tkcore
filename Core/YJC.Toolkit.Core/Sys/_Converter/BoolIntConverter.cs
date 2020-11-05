namespace YJC.Toolkit.Sys
{
    public sealed class BoolIntConverter : ITkTypeConverter
    {
        #region ITkTypeConverter 成员

        public string DefaultValue
        {
            get
            {
                return "0";
            }
        }

        public object ConvertFromString(string text, ReadSettings settings)
        {
            switch (text)
            {
                case "0":
                    return false;
                case "1":
                    return true;
                default:
                    return false;
            }
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            bool boolValue = (bool)value;
            return boolValue ? "1" : "0";
        }

        #endregion
    }
}
