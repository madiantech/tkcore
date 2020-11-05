using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite.Schema
{
    public sealed class AllInfoNode : BaseInfoNode, IFileCreatorNode, IFileCreator, IJudgeNode, ISchemaData
    {
        private readonly XmlSchemaAll fAll;
        private readonly List<BaseNode> fChildren;
        private bool fIsRequired;
        private readonly bool fDynamic;

        public AllInfoNode(ElementNode parentNode, XmlSchemaAll all, XmlSchemaSet schemaSet, HashSet<string> parents)
            : base(parentNode, "任一")
        {
            fAll = all;
            fChildren = new List<BaseNode>(all.Items.Count);
            fIsRequired = all.MinOccurs == 1;
            if (all.Parent is XmlSchemaComplexType complexType)
            {
                if (complexType?.Name == "ConstraintsType")
                    fDynamic = true;
            }
            foreach (XmlSchemaElement element in all.Items)
            {
                string name = element.QualifiedName.Name;
                if (parents.Contains(name))
                {
                    fChildren.Add(new RecursionNode(this, name, element.MinOccurs == 1));
                }
                else
                {
                    ElementNode node = new ElementNode(this, element, schemaSet, parents);
                    fChildren.Add(node);
                }
            }
        }

        public override bool IsRequired
        {
            get
            {
                return fIsRequired;
            }
        }

        public override IEnumerable<ITreeNode> Children
        {
            get
            {
                return fChildren;
            }
        }

        public NodeType NodeType { get; private set; }

        private string GetFileName()
        {
            if (fChildren.Count > 0)
            {
                if (fChildren[0] is ElementNode element)
                {
                    return element.RealName + ".md";
                }
            }
            return null;
        }

        public void CreateContent(StringBuilder builder, int level, bool isRoot)
        {
            string fileName = null;
            if (fDynamic)
                fileName = GetFileName();

            foreach (var node in fChildren)
                if (node is IFileCreatorNode fileCreator)
                    if (fDynamic)
                    {
                        if (fileName != null)
                            fileCreator.CreateRefContent(builder, fileName, level);
                    }
                    else
                        fileCreator.CreateContent(builder, level, false);
        }

        public void CreateRefContent(StringBuilder builder, string fileName, int level)
        {
        }

        public override IEnumerable<BaseNode> GetChildNodes()
        {
            return fChildren;
        }

        public void CreateMdFile(string path)
        {
            if (fDynamic)
            {
                string fileName = GetFileName();
                if (fileName == null)
                    return;
                fileName = Path.Combine(path, fileName);
                if (File.Exists(fileName))
                    return;

                StringBuilder builder = new StringBuilder();

                SchemaSuiteUtil.AppendLineFrontMatter(builder);

                foreach (var node in fChildren)
                    if (node is IFileCreatorNode fileCreator)
                        fileCreator.CreateContent(builder, 1, true);

                FileUtil.VerifySaveFile(fileName, builder.ToString(), Encoding.UTF8);
            }
        }

        public void Execute(int level)
        {
            // 判断附件
            if (fChildren.Count == 4)
            {
                if (fChildren[0] is ElementNode attType && fChildren[1] is ElementNode attName
                    && fChildren[2] is ElementNode property && fChildren[3] is ElementNode fileName)
                {
                    if (attType.RealName == SchemaConst.ATTACH_TYPE && attName.RealName == SchemaConst.ATTACH_NAME
                        && property.RealName == SchemaConst.ATTACH_PROPERTY && fileName.RealName == SchemaConst.ATTACH_FILE_NAME)
                    {
                        NodeType = NodeType.Attachment;
                        return;
                    }
                }
            }
            NodeType = NodeType.None;
            foreach (var item in Children)
            {
                if (item is IJudgeNode judge)
                    judge.Execute(level);
            }
        }

        public void AddMetaData(SchemaTable schemaTable, PageData pageData)
        {
            foreach (var node in fChildren)
                if (node is ISchemaData schema)
                    schema.AddMetaData(schemaTable, pageData);
        }

        public void AddData(Dictionary<string, object> data)
        {
            foreach (var node in fChildren)
                if (node is ISchemaData schema)
                    schema.AddData(data);
        }
    }
}