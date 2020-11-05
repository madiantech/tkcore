using System;
using System.Dynamic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class ModelTypeInfo
    {
        public ModelTypeInfo(Type type)
        {
            TkDebug.AssertArgumentNull(type, nameof(type), null);

            Type = type;
            IsStrongType = type != typeof(ExpandoObject) && !type.IsAnonymousType();
            TemplateType = IsStrongType ? type : typeof(ExpandoObject);
            TemplateTypeName = IsStrongType ? GetFriendlyName(Type) : "dynamic";
        }

        public bool IsStrongType { get; }

        public Type Type { get; }

        public Type TemplateType { get; }

        public string TemplateTypeName { get; }

        private static string GetGenericArguments(Type type)
        {
            var arr = (from item in type.GetGenericArguments()
                       select GetFriendlyName(item)).ToArray();
            return string.Join(", ", arr);
        }

        public object CreateTemplateModel(object model)
        {
            return this.IsStrongType ? model : model.ToExpando();
        }

        private static string GetFriendlyName(Type type)
        {
            if (type.IsGenericType)
            {
                return $"{type.Namespace}.{type.Name.Split('`')[0]}<{GetGenericArguments(type)}>";
            }
            else
                return $"{type.Namespace}.{type.Name}";
        }
    }
}