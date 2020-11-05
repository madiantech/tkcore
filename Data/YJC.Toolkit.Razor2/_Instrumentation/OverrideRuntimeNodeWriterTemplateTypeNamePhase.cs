using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class OverrideRuntimeNodeWriterTemplateTypeNamePhase : RazorEnginePhaseBase
    {
        internal class RuntimeNodeWriterTemplateTypeNameCodeTarget : CodeTarget
        {
            private readonly CodeTarget fTarget;
            private readonly string fTemplateTypeName;

            public RuntimeNodeWriterTemplateTypeNameCodeTarget(CodeTarget target, string templateTypeName)
            {
                fTarget = target;
                fTemplateTypeName = templateTypeName;
            }

            public override IntermediateNodeWriter CreateNodeWriter()
            {
                var writer = fTarget.CreateNodeWriter();
                if (writer is RuntimeNodeWriter runtimeNodeWriter)
                {
                    runtimeNodeWriter.TemplateTypeName = fTemplateTypeName;
                }

                return writer;
            }

            public override TExtension GetExtension<TExtension>()
            {
                return fTarget.GetExtension<TExtension>();
            }

            public override bool HasExtension<TExtension>()
            {
                return fTarget.HasExtension<TExtension>();
            }
        }

        private readonly string fTemplateTypeName;

        public OverrideRuntimeNodeWriterTemplateTypeNamePhase(string templateTypeName)
        {
            fTemplateTypeName = templateTypeName;
        }

        protected override void ExecuteCore(RazorCodeDocument codeDocument)
        {
            var documentNode = codeDocument.GetDocumentIntermediateNode();
            ThrowForMissingDocumentDependency(documentNode);

            documentNode.Target = new RuntimeNodeWriterTemplateTypeNameCodeTarget(documentNode.Target, fTemplateTypeName);
        }

        public static void Register(RazorProjectEngineBuilder builder)
        {
            var defaultRazorCSharpLoweringPhase = builder.Phases.SingleOrDefault(x =>
            {
                var type = x.GetType();
                var assemblyQualifiedNameOfTypeWeCareAbout = "Microsoft.AspNetCore.Razor.Language.DefaultRazorCSharpLoweringPhase, Microsoft.AspNetCore.Razor.Language, ";
                return type.AssemblyQualifiedName.Substring(0,
                    assemblyQualifiedNameOfTypeWeCareAbout.Length) == assemblyQualifiedNameOfTypeWeCareAbout;
            });

            TkDebug.AssertNotNull(defaultRazorCSharpLoweringPhase,
                "SetTemplateTypePhase cannot be registered as DefaultRazorCSharpLoweringPhase could not be located", null);

            // This phase needs to run just before DefaultRazorCSharpLoweringPhase
            var phaseIndex = builder.Phases.IndexOf(defaultRazorCSharpLoweringPhase);
            builder.Phases.Insert(phaseIndex, new OverrideRuntimeNodeWriterTemplateTypeNamePhase("global::RazorLight.Razor.RazorLightHelperResult"));
        }
    }
}