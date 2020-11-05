using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class DefaultMetadataReferenceManager : IMetadataReferenceManager
    {
        private IAssemblyDirectoryFormatter fDirectoryFormatter = new DefaultAssemblyDirectoryFormatter();

        public DefaultMetadataReferenceManager()
        {
            AdditionalMetadataReferences = new HashSet<MetadataReference>();
            ExcludedAssemblies = new HashSet<string>();
        }

        public DefaultMetadataReferenceManager(IOptions<TkRazorOptions> options, IAssemblyDirectoryFormatter directoryFormatter)
            : this(options.Value.AdditionalMetadataReferences, options.Value.ExcludedAssemblies)
        {
            fDirectoryFormatter = directoryFormatter;
        }

        public DefaultMetadataReferenceManager(HashSet<MetadataReference> metadataReferences)
        {
            TkDebug.AssertArgumentNull(metadataReferences, nameof(metadataReferences), null);

            AdditionalMetadataReferences = metadataReferences;
            ExcludedAssemblies = new HashSet<string>();
        }

        public DefaultMetadataReferenceManager(HashSet<MetadataReference> metadataReferences, HashSet<string> excludedAssemblies)
        {
            TkDebug.AssertArgumentNull(metadataReferences, nameof(metadataReferences), null);
            TkDebug.AssertArgumentNull(excludedAssemblies, nameof(excludedAssemblies), null);

            AdditionalMetadataReferences = metadataReferences;
            ExcludedAssemblies = excludedAssemblies;
        }

        public HashSet<MetadataReference> AdditionalMetadataReferences { get; }

        public HashSet<string> ExcludedAssemblies { get; }

        public IReadOnlyList<MetadataReference> Resolve(Assembly assembly)
        {
            var dependencyContext = DependencyContext.Load(assembly);

            return Resolve(assembly, dependencyContext);
        }

        internal IReadOnlyList<MetadataReference> Resolve(Assembly assembly, DependencyContext dependencyContext)
        {
            var libraryPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            IEnumerable<string> references = null;
            if (dependencyContext == null)
            {
                var context = new HashSet<string>();
                var x = GetReferencedAssemblies(assembly, ExcludedAssemblies, context).Union(new Assembly[] { assembly }).ToArray();
                references = x.Select(p => fDirectoryFormatter.GetAssemblyDirectory(p));
            }
            else
            {
                //references = dependencyContext.CompileLibraries.SelectMany(library => library.ResolveReferencePaths());
                references = dependencyContext.CompileLibraries.Where(x => !ExcludedAssemblies.Contains(x.Name))
                    .SelectMany(library => library.ResolveReferencePaths());

                if (!references.Any())
                {
                    throw new ToolkitException("Can't load metadata reference from the entry assembly. " +
                                                  "Make sure PreserveCompilationContext is set to true in *.csproj file", this);
                }
            }

            var metadataReferences = new List<MetadataReference>();

            foreach (var reference in references)
            {
                if (libraryPaths.Add(reference))
                {
                    using (var stream = File.OpenRead(reference))
                    {
                        var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                        var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);

                        metadataReferences.Add(assemblyMetadata.GetReference(filePath: reference));
                    }
                }
            }

            if (AdditionalMetadataReferences.Any())
            {
                metadataReferences.AddRange(AdditionalMetadataReferences);
            }

            return metadataReferences;
        }

        private static IEnumerable<Assembly> GetReferencedAssemblies(Assembly a, IEnumerable<string> excludedAssemblies, HashSet<string> visitedAssemblies = null)
        {
            visitedAssemblies = visitedAssemblies ?? new HashSet<string>();
            if (!visitedAssemblies.Add(a.GetName().EscapedCodeBase))
            {
                yield break;
            }

            foreach (var assemblyRef in a.GetReferencedAssemblies())
            {
                if (visitedAssemblies.Contains(assemblyRef.EscapedCodeBase)) { continue; }

                if (excludedAssemblies.Any(s => s.Contains(assemblyRef.Name))) { continue; }
                var loadedAssembly = Assembly.Load(assemblyRef);
                yield return loadedAssembly;
                foreach (var referenced in GetReferencedAssemblies(loadedAssembly, excludedAssemblies, visitedAssemblies))
                {
                    yield return referenced;
                }
            }
        }
    }
}