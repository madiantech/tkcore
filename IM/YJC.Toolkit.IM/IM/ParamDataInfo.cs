using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    internal class ParamDataInfo
    {
        private readonly ApiParameterAttribute fAttribute;

        public ParamDataInfo(ParameterInfo param, ApiParameterAttribute attribute)
        {
            Index = param.Position;
            ParamName = attribute.ParamName ?? StringUtil.GetName(attribute.NamingRule, param.Name);
            fAttribute = attribute;
            DictionaryName = attribute.DictionaryName
                             ?? StringUtil.GetName(attribute.NamingRule, param.Name);
            var converterAttr = Attribute.GetCustomAttribute(param,
                typeof(TkTypeConverterAttribute)) as TkTypeConverterAttribute;
            if (converterAttr != null)
                Converter = converterAttr.CreateTypeConverter(param.ParameterType);
        }

        public int Index { get; }

        public string ParamName { get; }

        public string ModelName { get => fAttribute.ModelName; }

        public string DictionaryName { get; }

        public ParamLocation Location { get => fAttribute.Location; }

        public ITkTypeConverter Converter { get; }

        public override string ToString() => $"参数{ParamName}";
    }
}