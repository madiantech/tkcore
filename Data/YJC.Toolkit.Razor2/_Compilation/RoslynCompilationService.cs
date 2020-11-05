using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using YJC.Toolkit.Sys;
using DependencyContextCompilationOptions = Microsoft.Extensions.DependencyModel.CompilationOptions;

namespace YJC.Toolkit.Razor
{
    internal class RoslynCompilationService : ICompilationService
    {
        private static readonly object fLocker = new object();

        private readonly IMetadataReferenceManager fMetadataReferenceManager;
        private readonly bool fIsDevelopment;
        private readonly Assembly fOperatingAssembly;
        private readonly List<MetadataReference> fMetadataReferences = new List<MetadataReference>();
        private CSharpParseOptions fParseOptions;
        private CSharpCompilationOptions fCompilationOptions;
        private bool fOptionsInitialized;

        public RoslynCompilationService(IMetadataReferenceManager referenceManager, Assembly operatingAssembly)
        {
            TkDebug.AssertArgumentNull(referenceManager, nameof(referenceManager), null);
            TkDebug.AssertArgumentNull(operatingAssembly, nameof(operatingAssembly), null);
            this.fMetadataReferenceManager = referenceManager;
            this.fOperatingAssembly = operatingAssembly;

            fIsDevelopment = IsAssemblyDebugBuild(OperatingAssembly);
            var pdbFormat = SymbolsUtility.SupportsFullPdbGeneration() ?
                DebugInformationFormat.Pdb :
                DebugInformationFormat.PortablePdb;

            EmitOptions = new EmitOptions(debugInformationFormat: pdbFormat);
        }

        public virtual Assembly OperatingAssembly
        {
            get
            {
                return fOperatingAssembly;
            }
        }

        public virtual EmitOptions EmitOptions { get; }

        public virtual CSharpCompilationOptions CSharpCompilationOptions
        {
            get
            {
                EnsureOptions();
                return fCompilationOptions;
            }
        }

        public virtual CSharpParseOptions ParseOptions
        {
            get
            {
                EnsureOptions();
                return fParseOptions;
            }
        }

        private void EnsureOptions()
        {
            lock (fLocker)
            {
                if (!fOptionsInitialized)
                {
                    var dependencyContextOptions = GetDependencyContextCompilationOptions();
                    fParseOptions = GetParseOptions(dependencyContextOptions);
                    fCompilationOptions = GetCompilationOptions(dependencyContextOptions);

                    fMetadataReferences.AddRange(fMetadataReferenceManager.Resolve(OperatingAssembly));

                    fOptionsInitialized = true;
                }
            }
        }

        private CSharpCompilation CreateCompilation(string compilationContent, string assemblyName)
        {
            SourceText sourceText = SourceText.From(compilationContent, Encoding.UTF8);
            SyntaxTree syntaxTree = CreateSyntaxTree(sourceText).WithFilePath(assemblyName);

            CSharpCompilation compilation = CreateCompilation(assemblyName).AddSyntaxTrees(syntaxTree);

            compilation = ExpressionRewriter.Rewrite(compilation);

            //var compilationContext = new RoslynCompilationContext(compilation);
            //_compilationCallback(compilationContext);
            //compilation = compilationContext.Compilation;
            return compilation;
        }

        private CSharpCompilationOptions GetCompilationOptions(DependencyContextCompilationOptions dependencyContextOptions)
        {
            var csharpCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            // Disable 1702 until roslyn turns this off by default
            csharpCompilationOptions = csharpCompilationOptions.WithSpecificDiagnosticOptions(
                new Dictionary<string, ReportDiagnostic>
                {
                    {"CS1701", ReportDiagnostic.Suppress}, // Binding redirects
                    {"CS1702", ReportDiagnostic.Suppress},
                    {"CS1705", ReportDiagnostic.Suppress}
                });

            if (dependencyContextOptions.AllowUnsafe.HasValue)
            {
                csharpCompilationOptions = csharpCompilationOptions.WithAllowUnsafe(
                    dependencyContextOptions.AllowUnsafe.Value);
            }

            OptimizationLevel optimizationLevel;
            if (dependencyContextOptions.Optimize.HasValue)
            {
                optimizationLevel = dependencyContextOptions.Optimize.Value ?
                    OptimizationLevel.Release : OptimizationLevel.Debug;
            }
            else
            {
                optimizationLevel = fIsDevelopment ?
                    OptimizationLevel.Debug : OptimizationLevel.Release;
            }
            csharpCompilationOptions = csharpCompilationOptions.WithOptimizationLevel(optimizationLevel);

            if (dependencyContextOptions.WarningsAsErrors.HasValue)
            {
                var reportDiagnostic = dependencyContextOptions.WarningsAsErrors.Value ?
                    ReportDiagnostic.Error : ReportDiagnostic.Default;
                csharpCompilationOptions = csharpCompilationOptions.WithGeneralDiagnosticOption(reportDiagnostic);
            }

            return csharpCompilationOptions;
        }

        private CSharpParseOptions GetParseOptions(DependencyContextCompilationOptions dependencyContextOptions)
        {
            var configurationSymbol = fIsDevelopment ? "DEBUG" : "RELEASE";
            var defines = dependencyContextOptions.Defines.Concat(new[] { configurationSymbol });

            var parseOptions = new CSharpParseOptions(preprocessorSymbols: defines);

            if (!string.IsNullOrEmpty(dependencyContextOptions.LanguageVersion))
            {
                if (LanguageVersionFacts.TryParse(dependencyContextOptions.LanguageVersion, out var languageVersion))
                {
                    parseOptions = parseOptions.WithLanguageVersion(languageVersion);
                }
                else
                {
                    Debug.Fail($"LanguageVersion {languageVersion} specified in the deps file could not be parsed.");
                }
            }

            return parseOptions;
        }

        private bool IsAssemblyDebugBuild(Assembly assembly)
        {
            return assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Select(da => da.IsJITTrackingEnabled).FirstOrDefault();
        }

        protected internal virtual DependencyContextCompilationOptions GetDependencyContextCompilationOptions()
        {
            var dependencyContext = DependencyContext.Load(OperatingAssembly);

            if (dependencyContext?.CompilationOptions != null)
            {
                return dependencyContext.CompilationOptions;
            }

            return DependencyContextCompilationOptions.Default;
        }

        public Assembly CompileAndEmit(IGeneratedRazorTemplate razorTemplate)
        {
            TkDebug.AssertArgumentNull(razorTemplate, nameof(razorTemplate), this);

            string assemblyName = Path.GetRandomFileName();
            var compilation = CreateCompilation(razorTemplate.GeneratedCode, assemblyName);

            using (var assemblyStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                var result = compilation.Emit(assemblyStream, pdbStream, options: EmitOptions);

                if (!result.Success)
                {
                    List<Diagnostic> errorsDiagnostics = result.Diagnostics
                        .Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error)
                        .ToList();
                    throw new RazorCompilerException(errorsDiagnostics,
                        razorTemplate.TemplateKey, razorTemplate.GeneratedCode);
                    ////StringBuilder builder = new StringBuilder();
                    ////builder.AppendLine("Failed to compile generated Razor template:");

                    ////var errorMessages = new List<string>();
                    ////foreach (Diagnostic diagnostic in errorsDiagnostics)
                    ////{
                    ////    FileLinePositionSpan lineSpan = diagnostic.Location.SourceTree.GetMappedLineSpan(diagnostic.Location.SourceSpan);
                    ////    string errorMessage = diagnostic.GetMessage();
                    ////    string formattedMessage = $"- ({lineSpan.StartLinePosition.Line}:{lineSpan.StartLinePosition.Character}) {errorMessage}";

                    ////    errorMessages.Add(formattedMessage);
                    ////    builder.AppendLine(formattedMessage);
                    ////}

                    ////builder.AppendLine("\nSee CompilationErrors for detailed information");

                    ////throw new TemplateCompilationException(builder.ToString(), errorMessages, this);
                }
                razorTemplate.ProjectItem.SaveAssembly(assemblyStream, pdbStream);

                assemblyStream.Seek(0, SeekOrigin.Begin);
                pdbStream.Seek(0, SeekOrigin.Begin);

                var assembly = Assembly.Load(assemblyStream.ToArray(), pdbStream.ToArray());

                return assembly;
            }
        }

        public CSharpCompilation CreateCompilation(string assemblyName)
        {
            return CSharpCompilation.Create(assemblyName, null, fMetadataReferences, CSharpCompilationOptions);
        }

        public SyntaxTree CreateSyntaxTree(SourceText sourceText)
        {
            return CSharpSyntaxTree.ParseText(sourceText, options: ParseOptions);
        }
    }
}