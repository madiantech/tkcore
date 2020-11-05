using System.Collections.Generic;

namespace YJC.Toolkit.MetaData
{
    public static class MetaDataExtension
    {
        private readonly static HashSet<ControlType> fTextControls = CreateTextCtrlSet();
        private readonly static HashSet<ControlType> fEmptyControls = CreateEmptyCtrlSet();
        private readonly static HashSet<ControlType> fDateControls = CreateDateCtrlSet();

        private static HashSet<ControlType> CreateEmptyCtrlSet()
        {
            HashSet<ControlType> result = new HashSet<ControlType>
            {
                ControlType.Text,
                ControlType.TextArea,
                ControlType.RichText,
                ControlType.Password,
                ControlType.DateTime,
                ControlType.Date,
                ControlType.Time,
                ControlType.EasySearch,
                ControlType.MultipleEasySearch,
                ControlType.Upload,
                ControlType.FolderPicker,
                ControlType.FontPicker,
                ControlType.FilePicker,
                ControlType.ColorPicker
            };
            return result;
        }

        private static HashSet<ControlType> CreateDateCtrlSet()
        {
            HashSet<ControlType> result = new HashSet<ControlType>
            {
                ControlType.DateTime,
                ControlType.Date,
                ControlType.Time
            };
            return result;
        }

        private static HashSet<ControlType> CreateTextCtrlSet()
        {
            HashSet<ControlType> result = new HashSet<ControlType>
            {
                ControlType.Text,
                ControlType.TextArea,
                ControlType.RichText,
                ControlType.Password
            };
            return result;
        }

        public static bool Contains(this PageStyle container, PageStyle style)
        {
            return (container & style) == style;
        }

        public static bool IsTextCtrl(this ControlType ctrlType)
        {
            return fTextControls.Contains(ctrlType);
        }

        public static bool IsDateCtrl(this ControlType ctrlType)
        {
            return fDateControls.Contains(ctrlType);
        }

        public static bool IsEmptyCtrl(this ControlType ctrlType)
        {
            return fEmptyControls.Contains(ctrlType);
        }
    }
}
