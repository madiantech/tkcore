using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class TemplateCompilationException : ToolkitException
    {
        public TemplateCompilationException(string message, IEnumerable<string> errors, object errorObject)
            : base(message, errorObject)
        {
            CompilationErrors = new List<string>();
            if (errors != null)
                CompilationErrors.AddRange(errors);
        }

        public List<string> CompilationErrors { get; }
    }
}