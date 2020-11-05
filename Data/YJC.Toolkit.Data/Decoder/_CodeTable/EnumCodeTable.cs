using System;
using System.Reflection;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public sealed class EnumCodeTable : DataCodeTable
    {
        private readonly Type fEnumType;
        private EnumCodeTableAttribute fAttribute;

        public EnumCodeTable(Type type, bool useIntValue)
        {
            TkDebug.AssertArgumentNull(type, "type", null);
            TkDebug.AssertArgument(type.IsEnum, "type", "参数type不是枚举类型", type);

            fEnumType = type;
            FieldInfo[] infos = type.GetFields();
            foreach (var field in infos)
            {
                if (!field.FieldType.IsEnum)
                    continue;

                DisplayNameAttribute attr = System.Attribute.GetCustomAttribute(field,
                    typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                string name = attr == null ? field.Name : attr.DisplayName;
                string value;
                if (useIntValue)
                    value = ((int)Enum.Parse(type, field.Name)).ToString(ObjectUtil.SysCulture);
                else
                    value = field.Name;
                Add(new CodeItem(value, name));
            }
        }

        public override BasePlugInAttribute Attribute
        {
            get
            {
                if (fAttribute == null)
                {
                    fAttribute = System.Attribute.GetCustomAttribute(fEnumType,
                        typeof(EnumCodeTableAttribute)).Convert<EnumCodeTableAttribute>();
                    if (fAttribute != null)
                        fAttribute.RegName = fAttribute.GetRegName(fEnumType);
                }
                return fAttribute;
            }
        }

        internal void SetAttribute(EnumCodeTableAttribute attribute)
        {
            fAttribute = attribute;
        }
    }
}