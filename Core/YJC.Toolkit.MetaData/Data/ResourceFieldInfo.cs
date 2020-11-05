using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class ResourceFieldInfo : IFieldInfo, IRegName, IReadObjectCallBack
    {
        #region IRegName 成员

        string IRegName.RegName
        {
            get
            {
                return NickName;
            }
        }

        #endregion

        #region IFieldInfo 成员

        [SimpleElement(NamespaceType.Toolkit)]
        public string FieldName { get; private set; }

        string IFieldInfo.DisplayName
        {
            get
            {
                if (DisplayName == null)
                    return NickName;
                else
                    return DisplayName.ToString();
            }
        }

        [SimpleElement(NamespaceType.Toolkit)]
        public string NickName { get; private set; }

        [SimpleAttribute]
        public TkDataType DataType { get; private set; }

        [SimpleAttribute]
        public bool IsKey { get; private set; }

        [SimpleAttribute]
        public bool IsAutoInc { get; private set; }

        #endregion

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (string.IsNullOrEmpty(NickName))
                NickName = StringUtil.GetNickName(FieldName);
        }

        #endregion

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText DisplayName { get; private set; }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "{{{0}, {1}}}", NickName, DataType);
        }
    }
}
