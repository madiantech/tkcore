using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    [RazorEngine(Author = "YJC", CreateDate = "2020-09-16",
        Description = "")]
    [InstancePlugIn, AlwaysCache]
    internal class TestSchemaRazorEngine
    {
        public static readonly IRazorEngine Instance = BuildRazor();

        private TestSchemaRazorEngine()
        {
        }

        private static IRazorEngine BuildRazor()
        {
            var builder = new RazorEngineBuilder().UseToolkitProject().UseBaseType("global::YJC.Toolkit.Razor.ToolkitTemplatePage<TModel>");
            var assembly = Assembly.GetAssembly(typeof(TestSchemaRazorEngine));
            builder.AddMetadataReferences(MetadataReference.CreateFromFile(assembly.Location));
            return builder.Build();
        }
    }
}