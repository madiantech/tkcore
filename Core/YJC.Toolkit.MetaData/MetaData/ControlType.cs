using YJC.Toolkit.Decoder;

namespace YJC.Toolkit.MetaData
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2015-08-11",
        Description = "控件类型的代码表", UseIntValue = false)]
    public enum ControlType
    {
        None,
        Text,
        Label,
        Date,
        DateTime,
        Time,
        Combo,
        ListBox,
        CheckBox,
        Password,
        EasySearch,
        CheckBoxList,
        MultipleEasySearch,
        RichText,
        Hidden,
        TextArea,
        Upload,
        RadioGroup,
        FolderPicker,
        FilePicker,
        FontPicker,
        ColorPicker,
        Custom
    }
}