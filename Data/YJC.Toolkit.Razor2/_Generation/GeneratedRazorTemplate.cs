using Microsoft.AspNetCore.Razor.Language;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class GeneratedRazorTemplate : IGeneratedRazorTemplate
    {
        public GeneratedRazorTemplate(TkRazorProjectItem projectItem, RazorCSharpDocument cSharpDocument)
        {
            TkDebug.AssertArgumentNull(projectItem, nameof(projectItem), null);
            TkDebug.AssertArgumentNull(cSharpDocument, nameof(cSharpDocument), null);

            ProjectItem = projectItem;
            CSharpDocument = cSharpDocument;
        }

        public TkRazorProjectItem ProjectItem { get; set; }

        public string TemplateKey => ProjectItem.Key;

        public RazorCSharpDocument CSharpDocument { get; set; }

        public string GeneratedCode => CSharpDocument.GeneratedCode;
    }
}