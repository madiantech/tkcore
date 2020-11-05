using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace YJC.Toolkit.Razor
{
    public static partial class RazorUtil
    {
        private static readonly Type DynamicType = typeof(DynamicObject);
        private static readonly Type ExpandoType = typeof(ExpandoObject);
        private static readonly Regex RegEx = new Regex(@"[^A-Za-z]*");

        internal static bool IsAnonymousType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return (type.IsClass
                    && type.IsSealed
                    && type.BaseType == typeof(object)
                    && type.Name.StartsWith("<>", StringComparison.Ordinal)
                    && type.IsDefined(typeof(CompilerGeneratedAttribute), true));
        }

        internal static bool IsDynamicType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return (DynamicType.IsAssignableFrom(type)
                    || ExpandoType.IsAssignableFrom(type)
                    || IsAnonymousType(type));
        }

        internal static string GenerateClassName()
        {
            Guid guid = Guid.NewGuid();
            return GenerateClassName(guid.ToString("N"));
        }

        internal static string GenerateClassName(string name)
        {
            return RegEx.Replace(name, string.Empty);
        }

        public static IEnumerable<string> GetAdditionAssemblies(Type type)
        {
            HashSet<string> result = new HashSet<string>();
            var assemblyAttrs = type.GetCustomAttributes(typeof(RequireAssemblyAttribute), false);
            if (assemblyAttrs != null)
                foreach (RequireAssemblyAttribute assemblyAttr in assemblyAttrs)
                {
                    foreach (string item in assemblyAttr.Assemblies)
                        result.Add(item);
                }

            return result;
        }
    }
}