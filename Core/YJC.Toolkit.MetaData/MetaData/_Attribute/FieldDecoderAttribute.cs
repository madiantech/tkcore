using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FieldDecoderAttribute : Attribute, IFieldDecoder
    {
        internal FieldDecoderAttribute()
        {
            Type = DecoderType.None;
        }

        public FieldDecoderAttribute(string regName)
            : this(DecoderType.CodeTable, regName)
        {
        }

        public FieldDecoderAttribute(DecoderType type, string regName)
        {
            TkDebug.AssertArgument((type != DecoderType.None && !string.IsNullOrEmpty(regName))
                || (type == DecoderType.None), "regName", "参数regName不能为空", null);

            Type = type;
            RegName = regName;
        }

        #region IFieldDecoder 成员

        [SimpleAttribute]
        public DecoderType Type { get; private set; }

        [SimpleAttribute]
        public string RegName { get; private set; }

        public IEnumerable<DecoderAdditionInfo> Additions
        {
            get
            {
                return null;
            }
        }

        #endregion

        public override string ToString()
        {
            if (Type == DecoderType.None)
                return "{None}";
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", Type, RegName);
        }
    }
}
