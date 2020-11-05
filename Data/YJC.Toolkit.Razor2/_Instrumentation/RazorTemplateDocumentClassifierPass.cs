using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace YJC.Toolkit.Razor
{
    internal class RazorTemplateDocumentClassifierPass : DocumentClassifierPassBase
    {
        public static readonly string RazorLightTemplateDocumentKind = "yjc.toolkit.razor.template";

        private readonly string fBaseType;

        public RazorTemplateDocumentClassifierPass(string baseType)
        {
            fBaseType = baseType;
        }

        protected override string DocumentKind => RazorLightTemplateDocumentKind;

        protected override bool IsMatch(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode) => true;

        protected override void OnDocumentStructureCreated(RazorCodeDocument codeDocument,
            NamespaceDeclarationIntermediateNode @namespace, ClassDeclarationIntermediateNode @class,
            MethodDeclarationIntermediateNode method)
        {
            string templateKey = codeDocument.Source.FilePath ?? codeDocument.Source.FilePath;

            base.OnDocumentStructureCreated(codeDocument, @namespace, @class, method);

            @namespace.Content = "YJC.Toolkit.Razor.CompiledTemplates";

            @class.ClassName = "GeneratedTemplate"; //CSharpIdentifier.GetClassNameFromPath(templateKey);
            @class.BaseType = fBaseType;
            @class.Modifiers.Clear();
            @class.Modifiers.Add("public");

            method.MethodName = "ExecuteAsync";
            method.Modifiers.Clear();
            method.Modifiers.Add("public");
            method.Modifiers.Add("async");
            method.Modifiers.Add("override");
            method.ReturnType = $"global::{typeof(System.Threading.Tasks.Task).FullName}";
        }
    }
}