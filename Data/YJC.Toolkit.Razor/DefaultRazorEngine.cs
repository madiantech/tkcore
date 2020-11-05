using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Razor
{
    internal class DefaultRazorEngine
    {
        private class NullRazorProjectFileSystem : RazorProjectFileSystem
        {
            public override IEnumerable<RazorProjectItem> EnumerateItems(string basePath)
            {
                throw new NotImplementedException();
            }

#if NETSTANDARD2_1

            public override RazorProjectItem GetItem(string path, string fileKind)
            {
                throw new NotImplementedException();
            }

#endif

#if NETSTANDARD2_1

            [System.Obsolete]
#endif

            public override RazorProjectItem GetItem(string path)
            {
                throw new NotImplementedException();
            }
        }

        public static RazorEngine CreateInstance(string baseType)
        {
            var configuration = RazorConfiguration.Default;
            var razorProjectEngine = RazorProjectEngine.Create(configuration,
                new NullRazorProjectFileSystem(), builder =>
                {
                    TkInjectDirective.Register(builder);
                    TkModelDirective.Register(builder);

                    if (!RazorLanguageVersion.TryParse("3.0", out var razorLanguageVersion)
                        || configuration.LanguageVersion.CompareTo(razorLanguageVersion) < 0)
                    {
                        NamespaceDirective.Register(builder);
                        FunctionsDirective.Register(builder);
                        InheritsDirective.Register(builder);
                    }
                    SectionDirective.Register(builder);

                    builder.Features.Add(new ModelExpressionPass());
                    builder.Features.Add(new RazorTemplateDocumentClassifierPass(baseType));
                    builder.Features.Add(new RazorAssemblyAttributeInjectionPass());
#if NETSTANDARD2_0
                   builder.Features.Add(new InstrumentationPass());
#endif
                    builder.AddTargetExtension(new TemplateTargetExtension()
                    {
                        TemplateTypeName = "global::YJC.Toolkit.Razor.TkRazorHelperResult",
                    });

                    OverrideRuntimeNodeWriterTemplateTypeNamePhase.Register(builder);
                });

            return razorProjectEngine.Engine;
        }

        //public static RazorEngine Instance
        //{
        //    get
        //    {
        //        var razorProjectEngine = RazorProjectEngine.Create(RazorConfiguration.Default,
        //            new NullRazorProjectFileSystem(), builder =>
        //        {
        //            TkInjectDirective.Register(builder);
        //            TkModelDirective.Register(builder);

        //            NamespaceDirective.Register(builder);
        //            FunctionsDirective.Register(builder);
        //            InheritsDirective.Register(builder);
        //            SectionDirective.Register(builder);

        //            builder.Features.Add(new ModelExpressionPass());
        //            builder.Features.Add(new RazorTemplateDocumentClassifierPass());
        //            builder.Features.Add(new RazorAssemblyAttributeInjectionPass());
        //            builder.Features.Add(new InstrumentationPass());
        //        });

        //        return razorProjectEngine.Engine;
        //    }
        //}
    }
}