using System;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class PropertyFieldInfo : IFieldInfoEx, IFieldInfo, IRegName
    {
        private PropertyFieldInfo(PropertyInfo property, FieldControlAttribute ctrlAttr)
        {
            NickName = property.Name;
            IsEmpty = true;
            DisplayNameAttribute dispAttr = Attribute.GetCustomAttribute(property,
                typeof(DisplayNameAttribute), false) as DisplayNameAttribute;
            if (dispAttr == null)
                DisplayName = NickName;
            else
                DisplayName = dispAttr.DisplayName;
            DataType = MetaDataUtil.ConvertTypeToDataType(property.PropertyType);
            Control = ctrlAttr;

            FieldLayoutAttribute layoutAttr = Attribute.GetCustomAttribute(property,
                typeof(FieldLayoutAttribute), false) as FieldLayoutAttribute;
            if (layoutAttr == null)
                Layout = new FieldLayoutAttribute();
            else
                Layout = layoutAttr;
            FieldDecoderAttribute decodeAttr = Attribute.GetCustomAttribute(property,
                typeof(FieldDecoderAttribute), false) as FieldDecoderAttribute;
            if (decodeAttr != null)
                Decoder = decodeAttr;
            else
                Decoder = new FieldDecoderAttribute();

            FieldInfoAttribute infoAttr = Attribute.GetCustomAttribute(property,
                typeof(FieldInfoAttribute), false) as FieldInfoAttribute;
            if (infoAttr != null)
            {
                IsKey = infoAttr.IsKey;
                IsEmpty = infoAttr.IsEmpty;
                Length = infoAttr.Length;
                Hint = infoAttr.Hint;
                HintPosition = infoAttr.HintPosition;
            }

            FieldUploadAttribute uploadAttr = Attribute.GetCustomAttribute(property,
                typeof(FieldUploadAttribute), false) as FieldUploadAttribute;
            Upload = uploadAttr;
        }

        #region IFieldInfo 成员

        [SimpleElement(NamespaceType.Toolkit)]
        public string FieldName
        {
            get
            {
                return NickName;
            }
        }

        [SimpleElement(NamespaceType.Toolkit)]
        public string DisplayName { get; private set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string NickName { get; private set; }

        [SimpleAttribute]
        public TkDataType DataType { get; private set; }

        [SimpleAttribute]
        public bool IsKey { get; private set; }

        public bool IsAutoInc
        {
            get
            {
                return false;
            }
        }

        #endregion IFieldInfo 成员

        #region IFieldInfoEx 成员

        public int Length { get; private set; }

        [SimpleAttribute]
        public bool IsEmpty { get; private set; }

        [SimpleAttribute]
        public int Precision { get; private set; }

        [SimpleAttribute]
        public HintPosition HintPosition { get; private set; }

        [SimpleAttribute(DefaultValue = FieldKind.Data)]
        public FieldKind Kind
        {
            get
            {
                return FieldKind.Data;
            }
        }

        public string Expression
        {
            get
            {
                return null;
            }
        }

        [ObjectElement(NamespaceType.Toolkit, ObjectType = typeof(FieldLayoutAttribute))]
        public IFieldLayout Layout { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, ObjectType = typeof(FieldControlAttribute))]
        public IFieldControl Control { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, ObjectType = typeof(FieldDecoderAttribute))]
        public IFieldDecoder Decoder { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, ObjectType = typeof(FieldUploadAttribute))]
        public IFieldUpload Upload { get; private set; }

        public bool IsShowInList(IPageStyle style, bool isInTable)
        {
            var ctrlAttr = Control.Convert<FieldControlAttribute>();
            if ((ctrlAttr.DefaultShow & PageStyle.List) != PageStyle.List)
                return false;

            if (isInTable)
                return ctrlAttr.Control != ControlType.Hidden;
            return true;
        }

        #endregion IFieldInfoEx 成员

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
        public string Hint { get; set; }

        public static PropertyFieldInfo Create(PropertyInfo property)
        {
            TkDebug.AssertArgumentNull(property, "property", null);

            FieldControlAttribute ctrlAttr = Attribute.GetCustomAttribute(property,
                typeof(FieldControlAttribute), false) as FieldControlAttribute;
            if (ctrlAttr == null)
                return null;
            return new PropertyFieldInfo(property, ctrlAttr);
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", NickName, DataType);
        }
    }
}