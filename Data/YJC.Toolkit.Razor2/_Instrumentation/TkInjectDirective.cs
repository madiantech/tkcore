using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class TkInjectDirective
    {
        private class Visitor : IntermediateNodeWalker
        {
            public ClassDeclarationIntermediateNode Class { get; private set; }

            public IList<DirectiveIntermediateNode> Directives { get; } = new List<DirectiveIntermediateNode>();

            public override void VisitClassDeclaration(ClassDeclarationIntermediateNode node)
            {
                if (Class == null)
                {
                    Class = node;
                }

                base.VisitClassDeclaration(node);
            }

            public override void VisitDirective(DirectiveIntermediateNode node)
            {
                if (node.Directive == Directive)
                {
                    Directives.Add(node);
                }
            }
        }

        internal class Pass : IntermediateNodePassBase, IRazorDirectiveClassifierPass
        {
            // Runs after the @model and @namespace directives
            public override int Order => 10;

            protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
            {
                var visitor = new Visitor();
                visitor.Visit(documentNode);
                var modelType = TkModelDirective.GetModelType(documentNode);

                var properties = new HashSet<string>(StringComparer.Ordinal);

                for (var i = visitor.Directives.Count - 1; i >= 0; i--)
                {
                    var directive = visitor.Directives[i];
                    var tokens = directive.Tokens.ToArray();
                    if (tokens.Length < 2)
                    {
                        continue;
                    }

                    var typeName = tokens[0].Content;
                    var memberName = tokens[1].Content;

                    if (!properties.Add(memberName))
                    {
                        continue;
                    }

                    typeName = typeName.Replace("<TModel>", "<" + modelType + ">");

                    var injectNode = new InjectIntermediateNode()
                    {
                        TypeName = typeName,
                        MemberName = memberName,
                    };

                    visitor.Class.Children.Add(injectNode);
                }
            }
        }

        internal class InjectTargetExtension : IInjectTargetExtension
        {
            private const string RazorInjectAttribute = "[global::YJC.Toolkit.Razor.RazorInjectAttribute]";

            public void WriteInjectProperty(CodeRenderingContext context, InjectIntermediateNode node)
            {
                TkDebug.AssertArgumentNull(context, nameof(context), this);
                TkDebug.AssertArgumentNull(node, nameof(node), this);

                var property = $"public {node.TypeName} {node.MemberName} {{ get; private set; }}";

                if (node.Source.HasValue)
                {
                    using (context.CodeWriter.BuildLinePragma(node.Source.Value))
                    {
                        context.CodeWriter.WriteLine(RazorInjectAttribute).WriteLine(property);
                    }
                }
                else
                {
                    context.CodeWriter.WriteLine(RazorInjectAttribute).WriteLine(property);
                }
            }
        }

        public static readonly DirectiveDescriptor Directive = DirectiveDescriptor.CreateDirective(
            "inject", DirectiveKind.SingleLine,
            builder =>
            {
                builder.AddTypeToken().AddMemberToken();
                builder.Usage = DirectiveUsage.FileScopedMultipleOccurring;
                builder.Description = ""; //TODO: add description
            });

        public static RazorProjectEngineBuilder Register(RazorProjectEngineBuilder builder)
        {
            TkDebug.AssertArgumentNull(builder, nameof(builder), null);

            builder.AddDirective(Directive);
            builder.Features.Add(new Pass());
            builder.AddTargetExtension(new InjectTargetExtension());
            return builder;
        }
    }
}