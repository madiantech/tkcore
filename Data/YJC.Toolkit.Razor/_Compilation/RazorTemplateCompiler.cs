using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class RazorTemplateCompiler : IRazorTemplateCompiler
    {
        private class ViewCompilerWorkItem
        {
            public bool SupportsCompilation { get; set; }

            public string NormalizedKey { get; set; }

            public IChangeToken ExpirationToken { get; set; }

            public CompiledTemplateDescriptor Descriptor { get; set; }

            public TkRazorProjectItem ProjectItem { get; set; }
        }

        private readonly RazorSourceGenerator fRazorSourceGenerator;
        private readonly RoslynCompilationService fCompiler;

        private readonly TkRazorOptions fRazorLightOptions;
        private readonly TkRazorProject fRazorProject;

        private readonly ConcurrentDictionary<string, string> fNormalizedKeysCache;

        public RazorTemplateCompiler(RazorSourceGenerator sourceGenerator,
            RoslynCompilationService roslynCompilationService, TkRazorProject razorLightProject,
            TkRazorOptions razorLightOptions)
        {
            TkDebug.AssertArgumentNull(sourceGenerator, nameof(sourceGenerator), null);
            TkDebug.AssertArgumentNull(roslynCompilationService, nameof(roslynCompilationService), null);
            //TkDebug.AssertArgumentNull(razorLightProject, nameof(razorLightProject), null);
            TkDebug.AssertArgumentNull(razorLightOptions, nameof(razorLightOptions), null);

            fRazorSourceGenerator = sourceGenerator;
            fCompiler = roslynCompilationService;
            fRazorProject = razorLightProject;
            fRazorLightOptions = razorLightOptions;

            fNormalizedKeysCache = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);
        }

        public ICompilationService CompilationService => fCompiler;

        internal ConcurrentDictionary<string, string> NormalizedKeysCache => fNormalizedKeysCache;

        internal RazorCompilerItem CreateDescriptor(string templateKey)
        {
            var item = CreateRuntimeCompilationWorkItem(templateKey).GetAwaiter().GetResult();

            var taskSource = new TaskCompletionSource<CompiledTemplateDescriptor>();
            if (item.SupportsCompilation)
            {
                // We'll compile in just a sec, be patient.
            }
            else
            {
                // If we can't compile, we should have already created the descriptor
                Debug.Assert(item.Descriptor != null);
                taskSource.SetResult(item.Descriptor);
            }

            // Now the lock has been released so we can do more expensive processing.
            if (item.SupportsCompilation)
            {
                Debug.Assert(taskSource != null);

                try
                {
                    CompiledTemplateDescriptor descriptor = CompileAndEmit(item.ProjectItem);
                    descriptor.ExpirationToken = item.ExpirationToken;
                    taskSource.SetResult(descriptor);
                }
                catch (Exception ex)
                {
                    taskSource.SetException(ex);
                }
            }

            return new RazorCompilerItem(taskSource.Task, item.ExpirationToken);
        }

        private async Task<ViewCompilerWorkItem> CreateRuntimeCompilationWorkItem(string templateKey)
        {
            TkRazorProjectItem projectItem = null;

            if (fRazorLightOptions.DynamicTemplates.TryGetValue(templateKey, out string templateContent))
            {
                projectItem = new TextSourceRazorProjectItem(templateKey, templateContent);
            }
            else
            {
                string normalizedKey = GetNormalizedKey(templateKey);
                projectItem = await fRazorProject.GetItemAsync(normalizedKey);
            }

            if (!projectItem.Exists)
            {
                throw new ToolkitException($"Project can not find template with key {projectItem.Key}", this);
            }

            return new ViewCompilerWorkItem()
            {
                SupportsCompilation = true,
                ProjectItem = projectItem,
                NormalizedKey = projectItem.Key,
                ExpirationToken = projectItem.ExpirationToken,
            };
        }

        internal string GetNormalizedKey(string templateKey)
        {
            Debug.Assert(templateKey != null);

            //Support path normalization only on Filesystem projects
            if (!(fRazorProject is FileSystemRazorProject))
            {
                return templateKey;
            }

            if (templateKey.Length == 0)
            {
                return templateKey;
            }

            if (!fNormalizedKeysCache.TryGetValue(templateKey, out var normalizedPath))
            {
                normalizedPath = NormalizeKey(templateKey);
                fNormalizedKeysCache[templateKey] = normalizedPath;
            }

            return normalizedPath;
        }

        protected string NormalizeKey(string templateKey)
        {
            if (!(fRazorProject is FileSystemRazorProject))
            {
                return templateKey;
            }

            var addLeadingSlash = templateKey[0] != '\\' && templateKey[0] != '/';
            var transformSlashes = templateKey.IndexOf('\\') != -1;

            if (!addLeadingSlash && !transformSlashes)
            {
                return templateKey;
            }

            var length = templateKey.Length;
            if (addLeadingSlash)
            {
                length++;
            }

            var builder = new InplaceStringBuilder(length);
            if (addLeadingSlash)
            {
                builder.Append('/');
            }

            for (var i = 0; i < templateKey.Length; i++)
            {
                var ch = templateKey[i];
                if (ch == '\\')
                {
                    ch = '/';
                }
                builder.Append(ch);
            }

            return builder.ToString();
        }

        protected virtual CompiledTemplateDescriptor CompileAndEmit(TkRazorProjectItem projectItem)
        {
            Assembly assembly = projectItem.TryLoadAssembly();
            if (assembly == null)
            {
                IGeneratedRazorTemplate generatedTemplate
                    = fRazorSourceGenerator.GenerateCodeAsync(projectItem).GetAwaiter().GetResult();
                assembly = fCompiler.CompileAndEmit(generatedTemplate);
            }

            // Anything we compile from source will use Razor 2.1 and so should have the new metadata.
            var loader = new RazorCompiledItemLoader();
            var item = loader.LoadItems(assembly).SingleOrDefault();
            var attribute = assembly.GetCustomAttribute<RazorTemplateAttribute>();

            return new CompiledTemplateDescriptor()
            {
                Item = item,
                TemplateKey = projectItem.Key,
                TemplateAttribute = attribute
            };
        }

        public Task<CompiledTemplateDescriptor> CompileAsync(string templateKey)
        {
            TkDebug.AssertArgumentNull(templateKey, nameof(templateKey), null);

            string normalizedPath = GetNormalizedKey(templateKey);
            Task<CompiledTemplateDescriptor> cachedResult = CacheManager.GetItem("RazorCompiler",
                normalizedPath, this).Convert<RazorCompilerItem>().Task;

            return cachedResult;
        }
    }
}