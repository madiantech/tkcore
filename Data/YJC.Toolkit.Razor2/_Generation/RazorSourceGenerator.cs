using Microsoft.AspNetCore.Razor.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class RazorSourceGenerator
    {
        public RazorSourceGenerator(RazorEngine projectEngine, TkRazorProject project = null, ISet<string> namespaces = null)
        {
            TkDebug.AssertArgumentNull(projectEngine, nameof(projectEngine), null);

            Namespaces = namespaces ?? new HashSet<string>();
            ProjectEngine = projectEngine;
            Project = project;
            DefaultImports = GetDefaultImports();
        }

        public RazorEngine ProjectEngine { get; set; }

        public TkRazorProject Project { get; set; }

        public ISet<string> Namespaces { get; set; }

        public RazorSourceDocument DefaultImports { get; set; }

        public async Task<IGeneratedRazorTemplate> GenerateCodeAsync(string key)
        {
            TkDebug.AssertArgumentNullOrEmpty(key, nameof(key), this);

            if (Project == null)
            {
                string _message = "Can not resolve a content for the template \"{0}\" as there is no project set." +
                                  "You can only render a template by passing it's content directly via string using coresponding function overload";

                throw new InvalidOperationException(_message);
            }

            TkRazorProjectItem projectItem = await Project.GetItemAsync(key).ConfigureAwait(false);
            return await GenerateCodeAsync(projectItem);
        }

        public async Task<IGeneratedRazorTemplate> GenerateCodeAsync(TkRazorProjectItem projectItem)
        {
            TkDebug.AssertArgumentNull(projectItem, nameof(projectItem), this);

            if (!projectItem.Exists)
            {
                throw new ToolkitException($"Project can not find template with key {projectItem.Key}", this);
            }

            RazorCodeDocument codeDocument = await CreateCodeDocumentAsync(projectItem);
            ProjectEngine.Process(codeDocument);

            RazorCSharpDocument document = codeDocument.GetCSharpDocument();
            if (document.Diagnostics.Count > 0)
            {
                var builder = new StringBuilder();
                builder.AppendLine("Failed to generate Razor template. See \"Diagnostics\" property for more details");

                foreach (RazorDiagnostic d in document.Diagnostics)
                {
                    builder.AppendLine($"- {d.GetMessage()}");
                }

                throw new TemplateGenerationException(builder.ToString(), document.Diagnostics, this);
            }

            return new GeneratedRazorTemplate(projectItem, document);
        }

        public virtual async Task<RazorCodeDocument> CreateCodeDocumentAsync(TkRazorProjectItem projectItem)
        {
            TkDebug.AssertArgumentNull(projectItem, nameof(projectItem), this);
            TkDebug.AssertArgument(projectItem.Exists, nameof(projectItem),
                $"Project can not find template with key {projectItem.Key}", this);

            using (var stream = projectItem.Read())
            {
                RazorSourceDocument source = RazorSourceDocument.ReadFrom(stream, projectItem.Key);
                IEnumerable<RazorSourceDocument> imports = await GetImportsAsync(projectItem);

                return RazorCodeDocument.Create(source, imports);
            }
        }

        public virtual async Task<IEnumerable<RazorSourceDocument>> GetImportsAsync(TkRazorProjectItem projectItem)
        {
            TkDebug.AssertArgumentNull(projectItem, nameof(projectItem), this);

            if (projectItem is TextSourceRazorProjectItem)
            {
                return Enumerable.Empty<RazorSourceDocument>();
            }

            var result = new List<RazorSourceDocument>();

            IEnumerable<TkRazorProjectItem> importProjectItems = await Project.GetImportsAsync(projectItem.Key);
            foreach (var importItem in importProjectItems)
            {
                if (importItem.Exists)
                {
                    using (var stream = importItem.Read())
                    {
                        result.Insert(0, RazorSourceDocument.ReadFrom(stream, null));
                    }
                }
            }

            if (Namespaces != null)
            {
                RazorSourceDocument namespacesImports = GetNamespacesImports();
                if (namespacesImports != null)
                {
                    result.Insert(0, namespacesImports);
                }
            }

            if (DefaultImports != null)
            {
                result.Insert(0, DefaultImports);
            }

            return result;
        }

        protected internal RazorSourceDocument GetDefaultImports()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (string line in GetDefaultImportLines())
                {
                    writer.WriteLine(line);
                }

                writer.Flush();

                stream.Position = 0;
                return RazorSourceDocument.ReadFrom(stream, fileName: null, encoding: Encoding.UTF8);
            }
        }

        protected internal RazorSourceDocument GetNamespacesImports()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (string @namespace in Namespaces)
                {
                    writer.WriteLine($"@using {@namespace}");
                }

                writer.Flush();

                stream.Position = 0;
                return RazorSourceDocument.ReadFrom(stream, fileName: null, encoding: Encoding.UTF8);
            }
        }

        public virtual IEnumerable<string> GetDefaultImportLines()
        {
            yield return "@using System";
            yield return "@using System.Collections.Generic";
            yield return "@using System.Linq";
            yield return "@using System.Threading.Tasks";

            //"@inject global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TModel> Html");
            //"@inject global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json");
            //"@inject global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component");
            //"@inject global::Microsoft.AspNetCore.Mvc.IUrlHelper Url");
            //"@inject global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider");
            //"@addTagHelper Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper, Microsoft.AspNetCore.Mvc.Razor");
            //"@addTagHelper Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper, Microsoft.AspNetCore.Mvc.Razor");
            //"@addTagHelper Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper, Microsoft.AspNetCore.Mvc.Razor");
        }
    }
}