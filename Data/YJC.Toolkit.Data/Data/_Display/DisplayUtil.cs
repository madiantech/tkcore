using System;
using System.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class DisplayUtil
    {
        public static bool IsNull(object value)
        {
            return value == null || value == DBNull.Value;
        }

        internal static object GetRowDecoderValue(IFieldValueProvider rowValue, string fieldName)
        {
            if (rowValue == null)
                return string.Empty;
            return rowValue[DecoderConst.DECODER_TAG + fieldName];
            //DataRow row = rowValue as DataRow;
            //if (row != null)
            //    return row.GetString(fieldName + "_Name");
            //ObjectContainer container = rowValue as ObjectContainer;
            //if (container != null)
            //    return container.Decoder.GetNameString(fieldName);

            //return string.Empty;
        }

        internal static void SetHrefDisplay(object display, string content)
        {
            IHrefDisplay href = display as IHrefDisplay;
            if (href != null)
                if (string.IsNullOrEmpty(href.Content))
                    href.Content = content;
        }

        public static object GetValue(string fieldName, DataRow row)
        {
            object value = null;
            try
            {
                value = row[fieldName];
            }
            catch
            {
            }
            return value;
        }

        public static string GetDisplayString(IConfigCreator<IDisplay> display, object value,
            Tk5FieldInfoEx field, IFieldValueProvider row)
        {
            TkDebug.AssertArgumentNull(display, "display", null);
            TkDebug.AssertArgumentNull(field, "field", null);

            var displayObj = display.CreateObject();
            return displayObj.DisplayValue(value, field, row);
        }

        public static object GetValue(string fieldName, ObjectContainer container)
        {
            if (container == null)
                return null;

            object value = null;
            try
            {
                value = container.MainObject.MemberValue(fieldName);
            }
            catch
            {
            }
            return value;
        }

        //public static string GetDisplayString(IConfigCreator<IDisplay> display, object value,
        //    Tk5FieldInfoEx field, ObjectContainer container)
        //{
        //    if (container == null)
        //        return string.Empty;

        //    TkDebug.AssertArgumentNull(display, "display", null);
        //    TkDebug.AssertArgumentNull(field, "field", null);

        //    var displayObj = display.CreateObject();
        //    return displayObj.DisplayValue(value, field, container.MainObject);
        //}
    }
}