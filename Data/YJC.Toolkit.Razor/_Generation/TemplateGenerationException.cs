using Microsoft.AspNetCore.Razor.Language;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class TemplateGenerationException : ToolkitException
    {
        public TemplateGenerationException(string message, IReadOnlyList<RazorDiagnostic> diagnostics,
            object errorObject) : base(message, errorObject)
        {
            Diagnostics = diagnostics;
        }

        public IReadOnlyList<RazorDiagnostic> Diagnostics { get; }
    }
}