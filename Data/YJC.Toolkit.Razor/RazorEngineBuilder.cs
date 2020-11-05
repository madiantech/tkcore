using Microsoft.CodeAnalysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class RazorEngineBuilder
    {
        public const string BASE_TYPE = "global::YJC.Toolkit.Razor.TemplatePage<TModel>";

        public Assembly OperatingAssembly { get; protected set; }

        public HashSet<string> Namespaces { get; protected set; }

        public ConcurrentDictionary<string, string> DynamicTemplates { get; protected set; }

        public HashSet<MetadataReference> MetadataReferences { get; protected set; }

        public HashSet<string> ExcludedAssemblies { get; protected set; }

        public List<Action<ITemplatePage>> PrerenderCallbacks { get; protected set; }

        public TkRazorProject Project { get; protected set; }

        public string BaseType { get; protected set; }

        public virtual RazorEngineBuilder UseProject(TkRazorProject project)
        {
            TkDebug.AssertArgumentNull(project, nameof(project), this);

            Project = project;
            return this;
        }

        public RazorEngineBuilder UseEmbeddedResourcesProject(Type rootType)
        {
            Project = new EmbeddedRazorProject(rootType);

            return this;
        }

        public RazorEngineBuilder UseEmbeddedResourcesProject(Assembly assembly, string rootNamespace = null)
        {
            Project = new EmbeddedRazorProject(assembly, rootNamespace);

            return this;
        }

        public RazorEngineBuilder UseFileSystemProject(string root)
        {
            Project = new FileSystemRazorProject(root);

            return this;
        }

        public RazorEngineBuilder UseFileSystemProject(string root, string extension)
        {
            Project = new FileSystemRazorProject(root, extension);

            return this;
        }

        public RazorEngineBuilder UseBaseType(string baseType)
        {
            TkDebug.AssertArgumentNullOrEmpty(baseType, nameof(baseType), this);

            BaseType = baseType;

            return this;
        }

        //public virtual RazorEngineBuilder UseMemoryCachingProvider()
        //{
        //    //CachingProvider = new MemoryCachingProvider();

        //    return this;
        //}

        //public virtual RazorEngineBuilder UseCachingProvider(ICachingProvider provider)
        //{
        //    TkDebug.AssertArgumentNull(provider, nameof(provider), this);

        //    CachingProvider = provider;

        //    return this;
        //}

        public virtual RazorEngineBuilder AddDefaultNamespaces(params string[] namespaces)
        {
            TkDebug.AssertArgumentNull(namespaces, nameof(namespaces), this);

            this.Namespaces = new HashSet<string>();

            foreach (string @namespace in namespaces)
            {
                this.Namespaces.Add(@namespace);
            }

            return this;
        }

        public virtual RazorEngineBuilder AddMetadataReferences(params MetadataReference[] metadata)
        {
            TkDebug.AssertArgumentNull(metadata, nameof(metadata), this);

            MetadataReferences = new HashSet<MetadataReference>();
            foreach (var reference in metadata)
            {
                MetadataReferences.Add(reference);
            }

            return this;
        }

        public virtual RazorEngineBuilder ExcludeAssemblies(params string[] assemblyNames)
        {
            TkDebug.AssertArgumentNull(assemblyNames, nameof(assemblyNames), this);

            ExcludedAssemblies = new HashSet<string>();
            foreach (var assemblyName in assemblyNames)
            {
                ExcludedAssemblies.Add(assemblyName);
            }

            return this;
        }

        public virtual RazorEngineBuilder AddPrerenderCallbacks(params Action<ITemplatePage>[] callbacks)
        {
            TkDebug.AssertArgumentNull(callbacks, nameof(callbacks), this);

            PrerenderCallbacks = new List<Action<ITemplatePage>>();
            PrerenderCallbacks.AddRange(callbacks);

            return this;
        }

        public virtual RazorEngineBuilder AddDynamicTemplates(IDictionary<string, string> dynamicTemplates)
        {
            TkDebug.AssertArgumentNull(dynamicTemplates, nameof(dynamicTemplates), this);

            this.DynamicTemplates = new ConcurrentDictionary<string, string>(dynamicTemplates);

            return this;
        }

        public virtual RazorEngineBuilder SetOperatingAssembly(Assembly assembly)
        {
            TkDebug.AssertArgumentNull(assembly, nameof(assembly), this);

            OperatingAssembly = assembly;

            return this;
        }

        public virtual IRazorEngine Build()
        {
            var options = new TkRazorOptions();

            if (Namespaces != null)
                options.Namespaces = Namespaces;
            if (DynamicTemplates != null)
                options.DynamicTemplates = DynamicTemplates;
            if (MetadataReferences != null)
                options.AdditionalMetadataReferences = MetadataReferences;
            if (ExcludedAssemblies != null)
                options.ExcludedAssemblies = ExcludedAssemblies;
            if (PrerenderCallbacks != null)
                options.PreRenderCallbacks = PrerenderCallbacks;
            string baseType = string.IsNullOrEmpty(BaseType) ? BASE_TYPE : BaseType;
            //if (CachingProvider != null)
            //    options.CachingProvider = CachingProvider;

            var metadataReferenceManager = new DefaultMetadataReferenceManager(
                options.AdditionalMetadataReferences, options.ExcludedAssemblies);
            var assembly = OperatingAssembly ?? Assembly.GetEntryAssembly();
            var compiler = new RoslynCompilationService(metadataReferenceManager, assembly);

            var sourceGenerator = new RazorSourceGenerator(DefaultRazorEngine.CreateInstance(baseType), Project, options.Namespaces);
            var templateCompiler = new RazorTemplateCompiler(sourceGenerator, compiler, Project, options);
            var templateFactoryProvider = new TemplateFactoryProvider();

            var engineHandler = new EngineHandler(options, templateCompiler, templateFactoryProvider);

            return new TkRazorEngine(engineHandler);
        }
    }
}