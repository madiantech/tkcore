using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static partial class HtmlExtension
    {
        public static string Control(this Tk5FieldInfoEx field, DataRow dataRow, DataSet dataSet, bool needId = true)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            IFieldValueProvider provider = new DataRowFieldValueProvider(dataRow, dataSet);
            return Control(field, provider, needId);
        }

        public static string Control(this Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", null);
            TkDebug.AssertArgumentNull(provider, "provider", null);

            ControlType ctrlType = field.InternalControl.SrcControl;
            string result = string.Empty;

            switch (ctrlType)
            {
                case ControlType.TextArea:
                    result = field.Textarea(provider, needId);
                    break;

                case ControlType.Combo:
                    result = field.Combo(provider, needId);
                    break;

                case ControlType.RadioGroup:
                    result = field.RadioGroup(provider, needId);
                    break;

                case ControlType.CheckBoxList:
                    result = field.CheckBoxList(provider, needId);
                    break;

                case ControlType.Text:
                case ControlType.Password:
                    result = field.Input(provider, needId);
                    break;

                case ControlType.CheckBox:
                    result = field.Switcher(provider, needId);
                    break;

                case ControlType.Date:
                    result = field.Date(provider, needId);
                    break;

                case ControlType.DateTime:
                    result = field.DateTime(provider, needId);
                    break;

                case ControlType.Time:
                    result = field.Time(provider, needId);
                    break;

                case ControlType.EasySearch:
                    result = field.EasySearch(provider, needId);
                    break;

                case ControlType.MultipleEasySearch:
                    result = field.MultipleEasySearch(provider, needId);
                    break;

                case ControlType.Label:
                    result = field.Detail(provider, true, needId);
                    break;

                case ControlType.RichText:
                    result = field.RichText(provider, needId);
                    break;

                case ControlType.Upload:
                    result = field.Upload(provider, needId);
                    break;
            }
            return result;
        }

        public static string Control(this Tk5FieldInfoEx field, ObjectContainer container,
            CodeTableContainer codeTables, bool needId = true)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            IFieldValueProvider provider = new ObjectContainerFieldValueProvider(container, codeTables);
            return Control(field, provider, needId);
        }

        public static string DisplayValue(this Tk5FieldInfoEx field, DataRow row)
        {
            IFieldValueProvider value = new DataRowFieldValueProvider(row,
                row == null ? null : row.Table.DataSet);
            return field.DisplayValue(value);
        }
    }
}