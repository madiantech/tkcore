using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Toolkit.SchemaSuite.Schema
{
    public sealed class ContentNode : BaseInfoNode, IFileCreatorNode
    {
        private readonly XmlSchemaSimpleContent fContent;
        private readonly SchemaAnnotation fAnnotation;
        private readonly XmlQualifiedName fType;

        public ContentNode(ElementNode parentNode, XmlSchemaSimpleContent content)
            : base(parentNode, "内容")
        {
            fContent = content;
            var anno = fContent.Annotation;
            if (content.Content is XmlSchemaSimpleContentExtension extension)
            {
                if (anno == null)
                    anno = extension.Annotation;
                fType = extension.BaseTypeName;
            }
            fAnnotation = new SchemaAnnotation(anno);
        }

        public override bool HasChildren
        {
            get
            {
                return false;
            }
        }

        public void CreateContent(StringBuilder builder, int level, bool isRoot)
        {
            string title = string.Empty.PadLeft(SchemaSuiteUtil.GetLevel(level + 1), '#');
            builder.Append($"{title} 节点内容").AppendLine();
            builder.Append($"- 类型: {fType.Name}").AppendLine();
            if (fAnnotation != null && !fAnnotation.IsEmpty)
                builder.Append($"- 说明: {fAnnotation}").AppendLine();
        }

        public void CreateRefContent(StringBuilder builder, string fileName, int level)
        {
        }
    }
}