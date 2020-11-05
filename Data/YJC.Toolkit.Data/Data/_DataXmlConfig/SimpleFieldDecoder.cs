using System;
using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Serializable]
    public class SimpleFieldDecoder : IFieldDecoder
    {
        private List<DecoderAdditionInfo> fAdditions;

        public SimpleFieldDecoder()
        {
            Type = DecoderType.None;
            RegName = null;
        }

        internal SimpleFieldDecoder(FieldConfigItem item)
        {
            if (item.CodeTable != null)
                SetProperties(item.CodeTable, DecoderType.CodeTable);
            else if (item.EasySearch != null)
                SetProperties(item.EasySearch, DecoderType.EasySearch);
            else
            {
                Type = DecoderType.None;
                RegName = null;
            }
        }

        internal SimpleFieldDecoder(DecoderConfigItem item, DecoderType type)
        {
            SetProperties(item, type);
        }

        #region IFieldDecoder ≥…‘±

        [SimpleAttribute]
        public DecoderType Type { get; private set; }

        [SimpleAttribute]
        public string RegName { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, CollectionType = typeof(List<DecoderAdditionInfo>),
            IsMultiple = true, UseConstructor = true)]
        public IEnumerable<DecoderAdditionInfo> Additions
        {
            get
            {
                return fAdditions;
            }
        }

        #endregion

        [ObjectElement(NamespaceType.Toolkit, CollectionType = typeof(List<EasySearchRefConfig>),
            IsMultiple = true, UseConstructor = true)]
        public IEnumerable<EasySearchRefConfig> RefFields { get; private set; }

        private void SetProperties(DecoderConfigItem item, DecoderType type)
        {
            Type = type;
            RegName = item.RegName;
            fAdditions = item.AdditionInfos;

            EasySearchConfigItem easySearch = item as EasySearchConfigItem;
            if (easySearch != null)
                RefFields = easySearch.RefFields;
        }

        public override string ToString()
        {
            if (Type == DecoderType.None)
                return "{None}";
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", Type, RegName);
        }
    }
}
