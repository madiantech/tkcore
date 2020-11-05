using System;

namespace YJC.Toolkit.Sys.Converter
{
    internal class StringArrayConverter : BaseSplitTypeConverter<string[]>
    {
        public static readonly ITkTypeConverter Converter = new StringArrayConverter();

        private StringArrayConverter()
            : base(',')
        {
        }

        //#region ITkTypeConverter 成员

        //public string DefaultValue
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}

        //public object ConvertFromString(string text, ReadSettings settings)
        //{
        //    try
        //    {
        //        text = text.Replace(',', ' ');
        //        string[] data = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
        //        for (int i = 0; i < data.Length; i++)
        //            data[i] = data[i].Trim();
        //        return data;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public string ConvertToString(object value, WriteSettings settings)
        //{
        //    string[] data = value as string[];
        //    if (data != null)
        //        return string.Join(",", data);
        //    return null;
        //}

        //#endregion ITkTypeConverter 成员

        protected override string[] Convert(string[] data)
        {
            return data;
        }
    }
}