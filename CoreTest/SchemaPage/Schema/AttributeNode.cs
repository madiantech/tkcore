using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite.Schema
{
    public sealed class AttributeNode : BaseNode, IFileCreatorNode
    {
        private readonly XmlSchemaAttribute fAttribute;
        private readonly string fName;
        private readonly bool fHasEnumType;
        private readonly EnumNode fEnumNode;
        private readonly bool fIsRequired;
        private readonly ElementNode fElementNode;
        private string fPascalName;
        private string fCamelName;
        private string fRealName;
        private readonly SchemaAnnotation fAnnotation;

        public AttributeNode(AttributeInfoNode parentNode, ElementNode elementNode, XmlSchemaAttribute attribute)
            : base(parentNode)
        {
            fElementNode = elementNode;
            fAttribute = attribute;
            fIsRequired = attribute.Use == XmlSchemaUse.Required;
            TypeCode = attribute.AttributeSchemaType.TypeCode;
            SchemaSuiteUtil.GetName(fAttribute.QualifiedName, out fRealName, out fPascalName, out fCamelName);
            IsCamel = fRealName == fCamelName;
            if (fIsRequired)
                fName = fRealName;
            else
                fName = fRealName + "(?)";
            var anno = attribute.Annotation ?? attribute.AttributeSchemaType.Annotation;
            fAnnotation = new SchemaAnnotation(anno);
            if (!fAnnotation.IsEmpty)
                fName += " " + fAnnotation;

            DefaultValue = attribute.DefaultValue;
            var content = attribute.AttributeSchemaType.Content as XmlSchemaSimpleTypeRestriction;
            if (content != null)
            {
                fHasEnumType = content.Facets.Count > 0 && content.Facets[0] is XmlSchemaEnumerationFacet;
                if (fHasEnumType)
                {
                    TypeName = attribute.AttributeSchemaType.Name;
                    if (string.IsNullOrEmpty(TypeName))
                        TypeName = string.Format(ObjectUtil.SysCulture, "{0}{1}Type",
                            elementNode.PascalName, fAttribute.QualifiedName.Name);
                    fEnumNode = new EnumNode(this, TypeName, content);
                }
            }
        }

        public bool IsCamel { get; private set; }

        public XmlTypeCode TypeCode { get; private set; }

        public string DefaultValue { get; private set; }

        public override bool HasChildren
        {
            get
            {
                return fHasEnumType;
            }
        }

        public override string Name
        {
            get
            {
                return fName;
            }
        }

        public override IEnumerable<ITreeNode> Children
        {
            get
            {
                if (fHasEnumType)
                    return EnumUtil.Convert(fEnumNode);
                else
                    return null;
            }
        }

        public override bool IsRequired
        {
            get
            {
                return fIsRequired;
            }
        }

        public string PascalName
        {
            get
            {
                return fPascalName;
            }
        }

        public string CamelName
        {
            get
            {
                return fCamelName;
            }
        }

        public string RealName
        {
            get
            {
                return fRealName;
            }
        }

        public string TypeName { get; private set; }

        public void CreateContent(StringBuilder builder, int level, bool isRoot)
        {
            if (fAnnotation.IsHide)
                return;

            string title = string.Empty.PadLeft(SchemaSuiteUtil.GetLevel(level + 1), '#');
            builder.Append($"{title} <font color=42b983>@</font>{fRealName}").AppendLine();
            builder.AppendFormat("- 必须: {0}", IsRequired ? "是" : "否").AppendLine();
            builder.Append($"- 类型: {SchemaSuiteUtil.GetType(fHasEnumType, fEnumNode, TypeCode)}").AppendLine();
            if (!fAnnotation.IsEmpty)
                builder.Append($"- 说明: {fAnnotation}").AppendLine();
        }

        public void CreateRefContent(StringBuilder builder, string fileName, int level)
        {
            throw new NotImplementedException();
        }
    }
}