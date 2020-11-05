using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Data;
using YJC.Toolkit.Data.Constraint;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [Serializable]
    public class Tk5FieldInfoEx : IFieldInfoEx, IRegName, ITk5FieldInfo
    {
        public Tk5FieldInfoEx(IFieldInfoEx field, IPageStyle style)
        {
            CopyFieldInfo(field);

            CopyFieldByType(field, style);
        }

        internal Tk5FieldInfoEx(Tk5FieldInfoEx field, string newNickName, IPageStyle style)
        {
            CopyFieldInfo(field);

            CopyFromTk5FieldInfo(field, newNickName, style);
        }

        internal Tk5FieldInfoEx(AddFieldConfig field, IPageStyle style)
        {
            FieldName = field.FieldName;
            NickName = field.NickName;
            DataType = field.DataType;
            Length = field.Length;
            IsEmpty = field.IsEmpty;
            PlaceHolder = field.PlaceHolder;
            Kind = field.Kind;
            Hint = FieldConfigItem.ToString(field.Hint, null);
            if (field.Hint != null)
                HintPosition = field.Hint.Position;
            DisplayName = FieldConfigItem.ToString(field.DisplayName, string.Empty);
            InternalControl = new SimpleFieldControl(field.Control, field.Order, style.Style);
            if (field.CodeTable != null)
                Decoder = new SimpleFieldDecoder(field.CodeTable, DecoderType.CodeTable);
            else if (field.EasySearch != null)
                Decoder = new SimpleFieldDecoder(field.EasySearch, DecoderType.EasySearch);
            else
                Decoder = new SimpleFieldDecoder();

            Extension = field.Extension;
            Layout = field.Layout;
            ResetExpression();
            SetPageProperties(field.Edit, field.ListDetail, style);
            if (ListDetail != null)
                ListDetail.TextHead = true;
            else
                ListDetail = new Tk5ListDetailConfig { TextHead = true };
            SetDisplay(style);
        }

        internal Tk5FieldInfoEx(PropertyFieldInfo field, IPageStyle style)
        {
            CopyFieldInfo(field);

            CopyFromProperyField(field, style);
        }

        internal Tk5FieldInfoEx(FieldConfigItem field, IPageStyle style)
        {
            CopyFieldInfo(field);

            CopyFromFieldConfigItem(field, style);
        }

        #region IFieldInfoEx 成员

        public int Length { get; internal set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool IsEmpty { get; set; }

        public int Precision { get; private set; }

        public FieldKind Kind { get; private set; }

        public string Expression { get; internal set; }

        [ObjectElement(NamespaceType.Toolkit, ObjectType = typeof(SimpleFieldLayout))]
        public IFieldLayout Layout { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, ObjectType = typeof(SimpleFieldControl))]
        public IFieldControl Control
        {
            get
            {
                return InternalControl;
            }
        }

        [ObjectElement(NamespaceType.Toolkit, ObjectType = typeof(SimpleFieldDecoder))]
        public IFieldDecoder Decoder { get; internal set; }

        IFieldUpload IFieldInfoEx.Upload
        {
            get
            {
                return Upload;
            }
        }

        public bool IsShowInList(IPageStyle style, bool isInTable)
        {
            return SchemeUtil.IsShowInList(Control, ListDetail, Kind, style, isInTable);
        }

        #endregion IFieldInfoEx 成员

        #region IFieldInfo 成员

        [SimpleElement(NamespaceType.Toolkit)]
        public string FieldName { get; private set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string DisplayName { get; internal set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string NickName { get; private set; }

        [SimpleAttribute]
        public TkDataType DataType { get; private set; }

        [SimpleAttribute]
        public bool IsKey { get; private set; }

        public bool IsAutoInc { get; private set; }

        #endregion IFieldInfo 成员

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return NickName;
            }
        }

        #endregion IRegName 成员

        [SimpleElement(NamespaceType.Toolkit)]
        public bool PlaceHolder { get; set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string Hint { get; set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public HintPosition HintPosition { get; set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5ListDetailConfig ListDetail { get; set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5EditConfig Edit { get; set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5ExtensionConfig Extension { get; internal set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5UploadConfig Upload { get; private set; }

        public object Tag { get; set; }

        public List<IConfigCreator<BaseConstraint>> Constraints { get; private set; }

        public SimpleFieldControl InternalControl { get; private set; }

        public string ControlName
        {
            get
            {
                SimpleFieldControl ctrl = InternalControl;
                if (ctrl.SrcControl == ControlType.Custom)
                {
                    TkDebug.AssertArgumentNullOrEmpty(ctrl.CustomControl, string.Format(
                        ObjectUtil.SysCulture, "字段{0}定义了Custom控件，需要明确定义CustomControl的类型，当前为空",
                        NickName), this);
                    return ctrl.CustomControl;
                }
                else
                    return ctrl.Control;
            }
        }

        public string JsControlName
        {
            get
            {
                SimpleFieldControl ctrl = InternalControl;
                if (ctrl.SrcControl == ControlType.Custom)
                    return ctrl.SrcControl.ToString();
                else
                    return ctrl.Control;
            }
        }

        private void CopyFieldByType(IFieldInfoEx field, IPageStyle style)
        {
            if (field is FieldConfigItem)
                CopyFromFieldConfigItem((FieldConfigItem)field, style);
            else if (field is PropertyFieldInfo)
                CopyFromProperyField((PropertyFieldInfo)field, style);
            else if (field is Tk5FieldInfoEx)
                CopyFromTk5FieldInfo((Tk5FieldInfoEx)field, string.Empty, style);
            else if (field is UnionFieldInfoEx)
                CopyFromUnionField((UnionFieldInfoEx)field, style);
            else
                CopyFromFieldInfoEx(field, style);
        }

        private void CopyFromFieldConfigItem(FieldConfigItem field, IPageStyle style)
        {
            PlaceHolder = field.PlaceHolder;
            Hint = FieldConfigItem.ToString(field.Hint, null);
            if (field.Hint != null)
                HintPosition = field.Hint.Position;

            SetFieldControl(field, style);
            Extension = field.Extension;
            Upload = field.Upload;
            Constraints = field.Constraints;
            SetPageProperties(field.Edit, field.ListDetail, style);
            SetDisplay(style);
        }

        private void CopyFromTk5FieldInfo(Tk5FieldInfoEx field, string newNickName, IPageStyle style)
        {
            if (!string.IsNullOrEmpty(newNickName))
                NickName = newNickName;
            PlaceHolder = field.PlaceHolder;
            Hint = field.Hint;
            HintPosition = field.HintPosition;

            ListDetail = field.ListDetail;
            Edit = field.Edit;
            Extension = field.Extension;
            Upload = field.Upload;
            Constraints = field.Constraints;
            InternalControl = field.InternalControl;
        }

        private void CopyFromUnionField(UnionFieldInfoEx field, IPageStyle style)
        {
            CopyFieldByType(field.SourceField, style);
        }

        private void CopyFromFieldInfoEx(IFieldInfoEx field, IPageStyle style)
        {
            SetFieldControl(field, style);
            SetDisplay(style);
        }

        private void CopyFromProperyField(PropertyFieldInfo field, IPageStyle style)
        {
            Hint = field.Hint;
            HintPosition = field.HintPosition;
            SetFieldControl(field, style);
            if (field.Upload != null)
                Upload = new Tk5UploadConfig(field.Upload);
            SetDisplay(style);
        }

        private void CopyFieldInfo(IFieldInfoEx field)
        {
            FieldName = field.FieldName;
            NickName = field.NickName;
            DataType = field.DataType;
            IsKey = field.IsKey;
            IsAutoInc = field.IsAutoInc;
            Length = field.Length;
            IsEmpty = field.IsEmpty;
            Precision = field.Precision;
            DisplayName = field.DisplayName;
            Decoder = field.Decoder;
            Expression = field.Expression;
            Layout = new SimpleFieldLayout(field.Layout);
            Kind = field.Kind;
        }

        private void SetFieldControl(IFieldInfoEx field, IPageStyle style)
        {
            InternalControl = new SimpleFieldControl(field.Control, style);
            SetControl(style);
        }

        private void SetPageProperties(Tk5EditConfig edit, Tk5ListDetailConfig listDetail, IPageStyle style)
        {
            switch (style.Style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                    Edit = Tk5EditConfig.Clone(edit);
                    ListDetail = Tk5ListDetailConfig.Clone(listDetail);
                    break;

                case PageStyle.Detail:
                case PageStyle.List:
                    ListDetail = Tk5ListDetailConfig.Clone(listDetail);
                    if (ListDetail != null && ListDetail.Link != null)
                        ListDetail.Link.ProcessType();
                    if (DataType == TkDataType.Text)
                    {
                        if (ListDetail == null)
                        {
                            ListDetail = new Tk5ListDetailConfig();
                            ListDetail.TextHead = true;
                        }
                    }
                    if (style.Style == PageStyle.List)
                        Edit = Tk5EditConfig.Clone(edit);
                    break;
            }
        }

        private void SetDisplay(IPageStyle style)
        {
            PageStyle pageStyle = style.Style;
            if (ListDetail == null)
                ListDetail = new Tk5ListDetailConfig();
            bool edit = pageStyle == PageStyle.Insert || pageStyle == PageStyle.Update
                || pageStyle == PageStyle.List;
            if (edit && Edit == null)
                Edit = new Tk5EditConfig();
            if (ListDetail.ListDisplay == null)
            {
                var display = GetListDisplay();
                if (display == null)
                    display = GetNormalDisplay();
                ListDetail.ListDisplay = display;
            }
            if (ListDetail.DetailDisplay == null)
                ListDetail.DetailDisplay = ListDetail.ListDisplay;
            if (edit)
            {
                if (Edit.Display == null)
                {
                    var display = GetNormalEditDisplay();
                    if (display == null)
                        display = GetNormalDisplay(false);
                    Edit.Display = display;
                }
            }
        }

        private static IConfigCreator<IDisplay> GetNormalDisplay()
        {
            return GetDisplay("<tk:NormalDisplay />");
        }

        internal static IConfigCreator<IDisplay> GetNormalDisplay(bool escape)
        {
            return GetDisplay(string.Format("<tk:NormalDisplay EscapeString=\"{0}\" />", escape));
        }

        internal IConfigCreator<IDisplay> GetListDisplay()
        {
            var display = GetNormalEditDisplay();
            if (display == null)
            {
                if (Upload != null)
                    display = GetDisplay("<tk:UploadDisplay />");
                else if (Decoder != null && Decoder.Type != DecoderType.None)
                {
                    if (InternalControl != null)
                    {
                        if (InternalControl.SrcControl == ControlType.MultipleEasySearch
                            || InternalControl.SrcControl == ControlType.CheckBoxList)
                            display = GetDisplay("<tk:MultipleDecoderDisplay />");
                        else
                            display = GetDisplay("<tk:DecoderDisplay />");
                    }
                }
                else if (InternalControl != null && InternalControl.SrcControl == ControlType.CheckBox)
                {
                    string checkValue = "1";
                    string uncheckValue = "0";
                    if (Extension != null)
                    {
                        if (!string.IsNullOrEmpty(Extension.CheckValue))
                            checkValue = Extension.CheckValue;
                        if (!string.IsNullOrEmpty(Extension.UnCheckValue))
                            uncheckValue = Extension.UnCheckValue;
                    }
                    string displayXml = string.Format(ObjectUtil.SysCulture,
                        "<tk:CheckedDisplay CheckValue='{0}' UncheckValue='{1}' />",
                        checkValue, uncheckValue);
                    display = GetDisplay(displayXml);
                }
                Tk5LinkConfig link = ListDetail.Link;
                if (link != null)
                {
                    link.ProcessType();
                    switch (link.RefType)
                    {
                        case LinkRefType.Href:
                            if (display == null)
                                display = GetNormalDisplay();
                            string displayXml = string.Format(ObjectUtil.SysCulture,
                                "<tk:HrefDisplay Base='{0}' Target='{1}'>{2}</tk:HrefDisplay>",
                                link.Base, link.Target, StringUtil.EscapeHtml(link.Content));
                            display = new LinkDisplayConfig(display, GetDecorateDisplay(displayXml));
                            break;

                        case LinkRefType.Http:
                            display = GetDisplay("<tk:HttpDisplay />");
                            break;

                        case LinkRefType.Email:
                            display = GetDisplay("<tk:MailToDisplay />");
                            break;
                    }
                }
            }
            return display;
        }

        internal IConfigCreator<IDisplay> GetNormalEditDisplay()
        {
            switch (DataType)
            {
                case TkDataType.Date:
                    return GetDisplay("<tk:DateDisplay />");

                case TkDataType.DateTime:
                    return GetDisplay("<tk:DateTimeDisplay />");

                case TkDataType.Double:
                case TkDataType.Money:
                    if (Extension != null && !string.IsNullOrEmpty(Extension.Format))
                    {
                        string display = string.Format(ObjectUtil.SysCulture,
                            "<tk:DoubleDisplay Format=\"{0}\" />", Extension.Format);
                        return GetDisplay(display);
                    }
                    break;
            }
            return null;
        }

        protected virtual string GetControl(ControlType control)
        {
            switch (control)
            {
                case ControlType.Label:
                    return "Detail";

                case ControlType.RichText:
                    return "HTML";

                case ControlType.Text:
                case ControlType.Date:
                case ControlType.DateTime:
                case ControlType.Time:
                case ControlType.Combo:
                case ControlType.CheckBox:
                case ControlType.Password:
                case ControlType.Hidden:
                case ControlType.TextArea:
                case ControlType.CheckBoxList:
                case ControlType.EasySearch:
                case ControlType.Upload:
                case ControlType.Custom:
                case ControlType.RadioGroup:
                case ControlType.MultipleEasySearch:
                    return control.ToString();

                case ControlType.ListBox:
                case ControlType.FolderPicker:
                case ControlType.FilePicker:
                case ControlType.FontPicker:
                case ControlType.ColorPicker:
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "系统不支持{0}控件", control), this);
                    break;
            }
            return string.Empty;
        }

        protected virtual string GetDetailControl(ControlType control)
        {
            switch (control)
            {
                case ControlType.CheckBoxList:
                case ControlType.MultipleEasySearch:
                    return "DetailMultiSelect";

                case ControlType.RichText:
                    return "DetailHTML";

                case ControlType.TextArea:
                    return "DetailTextArea";

                case ControlType.Label:
                case ControlType.Text:
                case ControlType.Date:
                case ControlType.DateTime:
                case ControlType.Time:
                case ControlType.Combo:
                case ControlType.CheckBox:
                case ControlType.Password:
                case ControlType.EasySearch:
                case ControlType.Custom:
                case ControlType.RadioGroup:
                    return "Detail";

                case ControlType.Hidden:
                    return control.ToString();

                case ControlType.ListBox:
                case ControlType.FolderPicker:
                case ControlType.FilePicker:
                case ControlType.FontPicker:
                case ControlType.ColorPicker:
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "系统不支持{0}控件", control), this);
                    break;
            }
            return string.Empty;
        }

        public bool ContainAttribute(string name, bool removeIfContain)
        {
            if (Edit != null && Edit.AttributeList != null)
            {
                var attr = (from item in Edit.AttributeList
                            where item.Name == name
                            select item).FirstOrDefault();
                if (attr != null)
                {
                    if (removeIfContain)
                        Edit.AttributeList.Remove(attr);

                    return true;
                }
            }
            return false;
        }

        internal void ResetExpression()
        {
            if (Extension != null)
                Expression = Extension.Expression;
            else
                Expression = null;
        }

        internal void SetControl(IPageStyle style)
        {
            if (style.Style == PageStyle.Detail)
                InternalControl.Control = GetDetailControl(InternalControl.SrcControl);
            else
                InternalControl.Control = GetControl(InternalControl.SrcControl);
        }

        internal void ProcessRefField(RegNameList<Tk5FieldInfoEx> fields)
        {
            if (Decoder == null)
                return;

            SimpleFieldDecoder decoder = Decoder as SimpleFieldDecoder;
            if (decoder == null || decoder.RefFields == null)
                return;

            foreach (var refField in decoder.RefFields)
            {
                Tk5FieldInfoEx refFieldInfo = fields[refField.RefField];
                if (refFieldInfo != null)
                    refField.CtrlType = refFieldInfo.InternalControl.Control;
                else
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "没有查找到{0}字段的定义，请确认", refField.RefField), this);
            }
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", NickName, DataType);
        }

        internal static IConfigCreator<IDecorateDisplay> GetDecorateDisplay(string xml)
        {
            return xml.ReadXmlFromFactory(DecorateDisplayConfigFactory.REG_NAME)
                as IConfigCreator<IDecorateDisplay>;
        }

        internal static IConfigCreator<IDisplay> GetDisplay(string xml)
        {
            return xml.ReadXmlFromFactory(CoreDisplayConfigFactory.REG_NAME)
                as IConfigCreator<IDisplay>;
        }
    }
}