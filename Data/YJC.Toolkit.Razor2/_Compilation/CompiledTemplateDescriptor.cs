using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.Extensions.Primitives;
using System;

namespace YJC.Toolkit.Razor
{
    public class CompiledTemplateDescriptor
    {
        public string TemplateKey { get; set; }

        public RazorTemplateAttribute TemplateAttribute { get; set; }

        public IChangeToken ExpirationToken { get; set; }

        public bool IsPrecompiled { get; set; }

        public RazorCompiledItem Item { get; set; }

        public Type Type => Item?.Type ?? TemplateAttribute?.TemplateType;
    }
}