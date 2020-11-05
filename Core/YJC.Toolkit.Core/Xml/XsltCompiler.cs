using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Xml;
using System.Xml.Xsl;

namespace YJC.Toolkit.Xml
{
    public sealed class XsltCompiler
    {
        private const string ASSEMBLY_NAME = "Toolkit.Template.Xslt";
        //private const string FileName = AssemblyName + ".dll";
        private static readonly ConstructorInfo GeneratedCodeCtor
            = typeof(GeneratedCodeAttribute).GetConstructor(new Type[] { typeof(string), typeof(string) });
        private static readonly ConstructorInfo SecurityTransparentCtor
            = typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes);
        private static readonly XmlReaderSettings ReaderSetting = CreateReaderSetting();
        private static readonly TypeAttributes Attributes = TypeAttributes.BeforeFieldInit
            | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.Public;

        private static XmlReaderSettings CreateReaderSetting()
        {
            XmlReaderSettings result = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                XmlResolver = XmlTransformUtil.Resolver
            };
            return result;
        }

        private AssemblyBuilder fAssBuilder;
        private ModuleBuilder fModBuilder;

        public XsltCompiler()
            : this(ASSEMBLY_NAME)
        {
        }

        public XsltCompiler(string assemblyName)
        {
            CreateBuilder(assemblyName);
        }

        private void CreateBuilder(string assemblyName)
        {
            AssemblyName asmName = new AssemblyName { Name = assemblyName };
            fAssBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            fAssBuilder.SetCustomAttribute(new CustomAttributeBuilder(SecurityTransparentCtor, new object[0]));
            fAssBuilder.SetCustomAttribute(new CustomAttributeBuilder(GeneratedCodeCtor,
                new object[] { assemblyName, "2.0.0.0" }));
            fModBuilder = fAssBuilder.DefineDynamicModule(assemblyName);
        }

        public Type CreateXsltType(string className, Stream xsltData)
        {
            XmlReader xsltReader = XmlReader.Create(xsltData, ReaderSetting);
            using (xsltReader)
            {
                TypeBuilder xsltType = fModBuilder.DefineType(className, Attributes);
                CompilerErrorCollection errors = XslCompiledTransform.CompileToType(xsltReader, XsltSettings.Default,
                    XmlTransformUtil.Resolver, false, xsltType, @"e:\a.script");
                if (errors.Count > 0)
                {
                    return null;
                }

                return xsltType.CreateType();
            }
        }
    }
}
