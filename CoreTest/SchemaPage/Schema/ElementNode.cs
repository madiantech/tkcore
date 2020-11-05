using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Xml;

namespace Toolkit.SchemaSuite.Schema
{
    [DebuggerDisplay("{Name}")]
    public class ElementNode : BaseNode, IFileCreatorNode, IJudgeNode, ISchemaData
    {
        private readonly XmlSchemaElement fElement;
        private readonly XmlSchemaSet fSchemaSet;
        private readonly bool fSimpleType;
        private readonly string fTypeName;
        private readonly List<BaseNode> fChildNodes;
        private readonly string fName;
        private readonly bool fHasEnumType;
        private readonly EnumNode fEnumNode;
        private readonly bool fHasChildren;
        private readonly bool fIsRequired;
        private readonly bool fIsMultiple;
        private string fPascalName;
        private string fCamelName;
        private string fRealName;
        private readonly AttributeInfoNode fAttributeInfoNode;
        private readonly BaseInfoNode fElementInfoNode;
        private readonly ContentNode fContentNode;
        private string fNamesapceUri;
        private readonly bool fHasContent;
        private readonly SchemaAnnotation fAnnotation;

        public ElementNode(BaseNode parentNode, XmlSchemaElement element, XmlSchemaSet schemaSet,
            HashSet<string> parents)
            : base(parentNode)
        {
            fElement = element;
            fSchemaSet = schemaSet;
            fSimpleType = element.ElementSchemaType is XmlSchemaSimpleType;
            fIsRequired = element.MinOccurs == 1;
            fIsMultiple = element.MaxOccursString == "unbounded";
            SchemaSuiteUtil.GetName(fElement.QualifiedName, out fRealName, out fPascalName, out fCamelName);
            IsCamel = fRealName == fCamelName;
            NamespaceUri = fElement.QualifiedName.Namespace;
            if (IsMultiple)
                fName = fRealName + (IsRequired ? "(+)" : "(*)");
            else
                fName = fRealName + (IsRequired ? "" : "(?)");

            var anno = element.Annotation ?? element.ElementSchemaType.Annotation;
            if (anno == null)
                anno = (fSchemaSet.GlobalElements[fElement.QualifiedName] as XmlSchemaElement)?.Annotation;

            fAnnotation = new SchemaAnnotation(anno);
            if (!fAnnotation.IsEmpty)
                fName += " " + fAnnotation;
            if (fSimpleType)
            {
                XmlSchemaSimpleType simpleType = element.ElementSchemaType as XmlSchemaSimpleType;
                fTypeName = simpleType.Name;
                TypeCode = simpleType.TypeCode;
                DefaultValue = element.DefaultValue;
                if (fTypeName == "EmptyType")
                    return;
                var content = simpleType.Content as XmlSchemaSimpleTypeRestriction;
                if (content != null)
                {
                    fHasEnumType = content.Facets.Count > 0 && content.Facets[0] is XmlSchemaEnumerationFacet;
                    if (fHasEnumType)
                    {
                        if (string.IsNullOrEmpty(fTypeName))
                            fTypeName = fRealName + "Type";
                        fEnumNode = new EnumNode(this, fTypeName, content);
                        fHasChildren = true;
                        fChildNodes = new List<BaseNode>();
                        fChildNodes.Add(fEnumNode);
                    }
                    else
                        fTypeName = string.Empty;
                }
                else
                    fTypeName = string.Empty;
            }
            else
            {
                fHasChildren = true;
                HashSet<string> currentParents = parents == null ? new HashSet<string>() : new HashSet<string>(parents);
                currentParents.Add(fRealName);

                XmlSchemaComplexType complexType = element.ElementSchemaType as XmlSchemaComplexType;
                fTypeName = complexType.Name;
                if (string.IsNullOrEmpty(fTypeName))
                    fTypeName = fElement.QualifiedName.Name; //+ "ConfigItem";
                fChildNodes = new List<BaseNode>();
                if (complexType.AttributeUses.Count > 0)
                {
                    AttributeInfoNode node = new AttributeInfoNode(this, complexType.AttributeUses);
                    fAttributeInfoNode = node;
                    fChildNodes.Add(node);
                }
                var content = complexType.ContentTypeParticle;
                if (content == null)
                    return;
                if (content is XmlSchemaSequence sequence)
                {
                    SequenceInfoNode node = new SequenceInfoNode(this, sequence, schemaSet, currentParents);
                    fElementInfoNode = node;
                    fChildNodes.Add(node);
                }
                else if (content is XmlSchemaChoice choice)
                {
                    ChoiceInfoNode node = new ChoiceInfoNode(this, choice, schemaSet, currentParents);
                    fElementInfoNode = node;
                    fChildNodes.Add(node);
                }
                else if (content is XmlSchemaAll all)
                {
                    AllInfoNode node = new AllInfoNode(this, all, schemaSet, currentParents);
                    fElementInfoNode = node;
                    fChildNodes.Add(node);
                }
                else
                {
                    if (complexType.DerivedBy == XmlSchemaDerivationMethod.Extension &&
                        complexType.ContentModel is XmlSchemaSimpleContent)
                    {
                        fContentNode = new ContentNode(this,
                            (XmlSchemaSimpleContent)complexType.ContentModel);
                        fHasContent = true;
                        fChildNodes.Add(fContentNode);
                    }
                }
            }
        }

        public XmlTypeCode TypeCode { get; private set; }

        public bool IsCamel { get; private set; }

        public bool HasContent
        {
            get
            {
                return fHasContent;
            }
        }

        public NamespaceType NamespaceType { get; private set; }

        public string DefaultValue { get; private set; }

        public string NamespaceUri
        {
            get
            {
                return fNamesapceUri;
            }
            set
            {
                fNamesapceUri = value;
                if (string.IsNullOrEmpty(value))
                {
                    NamespaceType = NamespaceType.None;
                }
                else if (value == ToolkitConst.NAMESPACE_URL)
                {
                    NamespaceType = NamespaceType.Toolkit;
                }
                else
                {
                    NamespaceType = NamespaceType.Namespace;
                }
            }
        }

        public bool SimpleType
        {
            get
            {
                return fSimpleType;
            }
        }

        public override bool IsRequired
        {
            get
            {
                return fIsRequired;
            }
        }

        public override bool IsMultiple
        {
            get
            {
                if (Parent is GroupBaseInfoNode parent)
                    return parent.IsMultiple || fIsMultiple;
                return fIsMultiple;
            }
        }

        public override IEnumerable<ITreeNode> Children
        {
            get
            {
                if (HasChildren)
                    return fChildNodes;
                else
                    return null;
            }
        }

        public override bool HasChildren
        {
            get
            {
                return fHasChildren;
            }
        }

        public override string Name
        {
            get
            {
                return fName;
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

        public int Level { get; set; }

        public string RealName
        {
            get
            {
                return fRealName;
            }
        }

        public string TypeName { get => fTypeName; }

        public IEnumerable<AttributeNode> Attributes
        {
            get
            {
                if (fAttributeInfoNode == null)
                    return Enumerable.Empty<AttributeNode>();
                return GetAttributeNodes();
            }
        }

        public IEnumerable<BaseNode> Elements
        {
            get
            {
                if (fElementInfoNode == null)
                    return Enumerable.Empty<ElementNode>();
                return fElementInfoNode.GetChildNodes();
            }
        }

        public NodeType NodeType { get; private set; }

        private IEnumerable<AttributeNode> GetAttributeNodes()
        {
            foreach (AttributeNode node in fAttributeInfoNode.Children)
                yield return node;
        }

        public void CreateContent(StringBuilder builder, int level, bool isRoot)
        {
            if (fAnnotation.IsHide)
                return;

            string title = string.Empty.PadLeft(SchemaSuiteUtil.GetLevel(level + 1), '#');
            builder.Append($"{title} {fRealName}").AppendLine();
            if (!isRoot)
                builder.AppendFormat("- 必须: {0}", IsRequired ? "是" : "否").AppendLine();
            if (!isRoot && IsMultiple)
                builder.Append("- 重复: 是").AppendLine();
            if (SimpleType)
            {
                if (string.IsNullOrEmpty(fTypeName))
                    builder.Append($"- 类型: {SchemaSuiteUtil.GetType(fHasEnumType, fEnumNode, TypeCode)}").AppendLine();
                if (!fAnnotation.IsEmpty)
                    builder.Append($"- 说明: {fAnnotation}").AppendLine();
            }
            else
            {
                if (!fAnnotation.IsEmpty)
                    builder.Append($"- 说明: {fAnnotation}").AppendLine();
                if (HasChildren)
                    foreach (var node in fChildNodes)
                        if (node is IFileCreatorNode fileCreator)
                            fileCreator.CreateContent(builder, level + 1, false);
            }
        }

        public void CreateRefContent(StringBuilder builder, string fileName, int level)
        {
            if (fAnnotation.IsHide)
                return;

            string title = string.Empty.PadLeft(SchemaSuiteUtil.GetLevel(level + 1), '#');
            builder.Append($"{title} {fRealName}").AppendLine();

            string fileNameNoExt = fileName.Replace(".md", "");
            string refText = $"[{fRealName}]({fileNameNoExt + ".html#" + fRealName.ToLower()})";

            if (IsMultiple)
                builder.Append("- 重复: 是").AppendLine();
            if (!fAnnotation.IsEmpty)
                builder.Append($"- 说明: {fAnnotation}").AppendLine();
            builder.AppendFormat($"- 参考: {refText}").AppendLine();
        }

        public void Execute(int level)
        {
            Level = level;
            if (fSimpleType)
            {
                NodeType = NodeType.Normal;
            }
            else
            {
                NodeType = NodeType.None;
                if (fElementInfoNode is IJudgeNode judge)
                {
                    judge.Execute(level + 1);

                    switch (judge.NodeType)
                    {
                        case NodeType.None:
                            NodeType = NodeType.None;
                            break;

                        case NodeType.CheckBox:
                        case NodeType.Date:
                            NodeType = judge.NodeType;
                            break;

                        case NodeType.Attachment:
                            if (fAttributeInfoNode.IsOrderAttribute)
                                NodeType = judge.NodeType;
                            else
                                TkDebug.ThrowImpossibleCode(this);
                            break;

                        case NodeType.Normal:
                            TkDebug.ThrowImpossibleCode(this);
                            break;
                    }
                }
            }
        }

        public virtual void AddMetaData(SchemaTable schemaTable, PageData pageData)
        {
            switch (NodeType)
            {
                case NodeType.None:
                    if (!fSimpleType)
                    {
                        SectionGroup group = null;
                        if (Level == 2)
                        {
                            group = new SectionGroup(schemaTable.CurrentOrder, RealName);
                            pageData.Groups.Add(group);
                        }
                        if (fElementInfoNode is ISchemaData schema)
                            schema.AddMetaData(schemaTable, pageData);
                        if (Level == 2)
                        {
                            group.EndOrder = schemaTable.CurrentOrder;
                        }
                    }
                    break;

                case NodeType.Normal:
                    schemaTable.AddField(new SchemaField(this));
                    break;

                case NodeType.CheckBox:
                    schemaTable.AddField(new SchemaField(this));
                    break;

                case NodeType.Date:
                    schemaTable.AddField(new SchemaField(this));
                    break;

                case NodeType.Attachment:
                    schemaTable.AddField(new SchemaField(fAttributeInfoNode.Children.First().Convert<AttributeNode>()));
                    var list = fElementInfoNode.Children.Convert<List<BaseNode>>();
                    schemaTable.AddField(new SchemaField(list[0].Convert<ElementNode>()));
                    schemaTable.AddField(new SchemaField(list[1].Convert<ElementNode>()));
                    schemaTable.AddField(new SchemaField(list[2].Convert<ElementNode>()));
                    schemaTable.AddField(new SchemaField(list[3].Convert<ElementNode>()));
                    break;
            }
        }

        public virtual void AddData(Dictionary<string, object> data)
        {
            switch (NodeType)
            {
                case NodeType.None:
                    if (!fSimpleType)
                    {
                        Dictionary<string, object> subDict = new Dictionary<string, object>();
                        data.Add(RealName, subDict);
                        if (fElementInfoNode is ISchemaData schema)
                            schema.AddData(subDict);
                    }
                    break;

                case NodeType.Normal:
                    data.Add(RealName, string.Empty);
                    break;

                case NodeType.CheckBox:
                    data.Add(RealName, "0");
                    break;

                case NodeType.Date:
                    data.Add(RealName, DateTime.Today);
                    break;

                case NodeType.Attachment:

                    break;
            }
        }
    }
}