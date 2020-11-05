using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Razor;
using System.Web.Razor.Generator;
using Microsoft.CSharp;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal static class CompilerService
    {
        internal const string DEFAULT_NS = "YJC.Toolkit.Razor.Dynamic";

        private static readonly string[] DEFAULT_ASSEMBLIES = new string[] { "netstandard.dll", "System.dll",
            "System.Core.dll", "Microsoft.CSharp.dll", "YJC.Toolkit.Core.dll", "YJC.Toolkit.OldRazor.dll" };

        private static readonly string[] DEFAULT_NSES = new string[] { "System", "System.Text",
            "System.Collections.Generic", "System.Linq", "System.IO", "YJC.Toolkit.Razor" };

        //"System.Data.dll", "System.Xml.dll", "YJC.Toolkit.Core.Extension.dll", "YJC.Toolkit.MetaData.dll",
        //"YJC.Toolkit.Data.dll", "YJC.Toolkit.AdoData.dll" };

        private static RazorTemplateEngine CreateHost(Type baseClassType, string generatedClass,
            IEnumerable<string> referencedNamespaces)
        {
            RazorEngineHost host = new RazorEngineHost(new CSharpRazorCodeLanguage())
            {
                DefaultBaseClass = baseClassType.FullName,
                DefaultClassName = generatedClass,
                DefaultNamespace = DEFAULT_NS,
                GeneratedClassContext = new GeneratedClassContext("Execute", "Write", "WriteLiteral",
                    "WriteTo", "WriteLiteralTo", null, "DefineSection")
            };

            foreach (string ns in DEFAULT_NSES)
                host.NamespaceImports.Add(ns);

            if (referencedNamespaces != null)
                foreach (string ns in referencedNamespaces)
                    host.NamespaceImports.Add(ns);

            return new RazorTemplateEngine(host);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static CompilerResults InternalCompile(string fileName, RazorConfiguration config,
            GeneratorResults razorResults, CSharpCodeProvider codeProvider, CompilerParameters compilerParameters)
        {
            if (fileName != null && config.SaveCompileAssembly)
                compilerParameters.OutputAssembly = Path.Combine(config.SavePath, fileName + ".dll");

            CompilerResults compilerResults = codeProvider.CompileAssemblyFromDom(compilerParameters,
                razorResults.GeneratedCode);
            return compilerResults;
        }

        private static CompilerResults Complie(Type baseClassType, TextReader sourceReader,
            string generatedClass, string fileName, IEnumerable<string> referNamespaces,
            IEnumerable<string> referAssemblies, RazorConfiguration config, out string generatedCode)
        {
            RazorTemplateEngine engine = CreateHost(baseClassType, generatedClass, referNamespaces);

            // Generate the template class as CodeDom
            GeneratorResults razorResults = engine.GenerateCode(sourceReader);

            // Create code from the codeDom and compile
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions();

            using (StringWriter writer = new StringWriter(ObjectUtil.SysCulture))
            {
                codeProvider.GenerateCodeFromCompileUnit(razorResults.GeneratedCode, writer, options);
                generatedCode = writer.ToString();
                if (fileName != null && config.SaveCompileCode)
                {
                    string path = Path.Combine(config.SavePath, fileName + ".cs");
                    try
                    {
                        File.WriteAllText(path, generatedCode);
                    }
                    catch
                    {
                    }
                }
            }

            CompilerParameters compilerParameters = new CompilerParameters
            {
                IncludeDebugInformation = BaseAppSetting.Current.IsDebug,
                CompilerOptions = "/optimize",
                TempFiles = new TempFileCollection(config.SavePath, true)
            };

            //// Also add the current assembly so RazorTemplateBase is available
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Substring(8));
            //compilerParameters.ReferencedAssemblies.Add(currentPath);
            foreach (string assembly in DEFAULT_ASSEMBLIES)
                compilerParameters.ReferencedAssemblies.Add(GetRefAssembly(assembly, currentPath));
            if (referAssemblies != null)
                foreach (string assembly in referAssemblies)
                    compilerParameters.ReferencedAssemblies.Add(GetRefAssembly(assembly, currentPath));

            return InternalCompile(fileName, config, razorResults, codeProvider, compilerParameters);
        }

        private static string GetRefAssembly(string assembly, string path)
        {
            if (assembly.StartsWith("System", StringComparison.OrdinalIgnoreCase) ||
                assembly.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase) ||
                assembly.StartsWith("netstandard", StringComparison.OrdinalIgnoreCase))
                return assembly;
            return Path.Combine(path, assembly);
        }

        private static string GenernateFileName(string razorTemplate, string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName))
                return null;
            string result = string.Format(ObjectUtil.SysCulture, "{1}_{0}",
                razorTemplate.GetHashCode(), RazorUtil.GenerateClassName(cacheName));
            return result;
        }

        public static Assembly ComplieToAssembly(Type baseClassType, string razorTemplate,
            string generatedClass, string cacheName, IEnumerable<string> referNamespaces,
            IEnumerable<string> referAssemblies, RazorConfiguration config)
        {
            string fileName = GenernateFileName(razorTemplate, cacheName);
            if (fileName != null)
            {
                string dllName = Path.Combine(config.SavePath, fileName + ".dll");
                if (File.Exists(dllName))
                {
                    try
                    {
                        AssemblyName name = AssemblyName.GetAssemblyName(dllName);
                        Assembly assembly = AppDomain.CurrentDomain.Load(name);
                        return assembly;
                    }
                    catch
                    {
                    }
                }
            }

            using (StringReader sourceReader = new StringReader(razorTemplate))
            {
                string generatedCode;
                CompilerResults results = Complie(baseClassType, sourceReader, generatedClass,
                    fileName, referNamespaces, referAssemblies, config, out generatedCode);

                if (results.Errors.Count > 0)
                {
                    if (config.RaiseOnCompileError)
                        throw new RazorCompilerException(results.Errors, razorTemplate, generatedCode);
                }
                return results.CompiledAssembly;
            }
        }
    }
}