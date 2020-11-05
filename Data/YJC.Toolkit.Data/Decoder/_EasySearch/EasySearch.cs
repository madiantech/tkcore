using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public abstract class EasySearch : IDecoder
    {
        public const int DEFAULT_TOP_COUNT = 15;

        private EasySearchAttribute fAttribute;

        protected EasySearch()
        {
            TopCount = DEFAULT_TOP_COUNT;
        }

        #region IDecoder 成员

        public abstract IDecoderItem Decode(string code, params object[] args);

        public void Fill(params object[] args)
        {
        }

        #endregion

        public virtual EasySearchAttribute Attribute
        {
            get
            {
                if (fAttribute == null)
                    fAttribute = System.Attribute.GetCustomAttribute(GetType(),
                        typeof(EasySearchAttribute)) as EasySearchAttribute;
                return fAttribute;
            }
        }

        public string RegName
        {
            get
            {
                EasySearchAttribute attr = Attribute;
                TkDebug.AssertNotNull(attr, "EasySearch对象没有附着对应的Attribute", this);
                return attr.GetRegName(GetType());
            }
        }

        public int TopCount { get; set; }

        public IFieldInfo ValueField { get; protected set; }

        public IFieldInfo NameField { get; protected set; }

        public IFieldInfo PinyinField { get; protected set; }

        public abstract IEnumerable<IDecoderItem> Search(SearchField field, string text, List<EasySearchRefField> refFields);

        public abstract IDecoderItem[] SearchByName(string name, params object[] args);

        private static SearchField InternalParseSearchValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return SearchField.DefaultValue;

            bool isChinese = false, isPy = true;
            foreach (char c in value)
            {
                if ((int)c > 255)
                {
                    isChinese = true;
                    break;
                }
                char lower = char.ToLower(c, ObjectUtil.SysCulture);
                if (lower < 'a' || lower > 'z')
                {
                    isPy = false;
                    break;
                }
            }
            if (isChinese)
                return SearchField.Name;
            if (isPy)
                return SearchField.Pinyin;

            return SearchField.Value;
        }

        protected virtual SearchField ParseSearchText(string text)
        {
            return InternalParseSearchValue(text);
        }

        public IEnumerable<IDecoderItem> Search(string text, List<EasySearchRefField> refFields)
        {
            SearchField field = ParseSearchText(text);
            return Search(field, text, refFields);
        }

        private static string SafeTrim(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return str.Trim();
        }

        public EasySearchErrorType CheckName(string name, ref string value, params object[] args)
        {
            if (!string.IsNullOrEmpty(value))
            {
                IDecoderItem item = Decode(value, args);
                // YJC 2012.9.21修改，此处为适应IE提交自动删除空格的问题
                //DataRow row = FindByKey(selector, tableName, value);
                if (item == null || SafeTrim(item.Name) != SafeTrim(name))
                {
                    value = string.Empty;
                    return EasySearchErrorType.NotExist;
                }
                else
                    return EasySearchErrorType.None;
            }
            if (string.IsNullOrEmpty(value))
            {
                IDecoderItem[] result = SearchByName(name, args);
                if (result == null || result.Length == 0)
                {
                    value = string.Empty;
                    return EasySearchErrorType.NotExist;
                }
                switch (result.Length)
                {
                    case 1:
                        value = result[0].Value;
                        return EasySearchErrorType.None;
                    default:
                        value = string.Empty;
                        return EasySearchErrorType.VariousTwo;

                }
            }
            value = string.Empty;
            return EasySearchErrorType.NotExist;
        }
    }
}
