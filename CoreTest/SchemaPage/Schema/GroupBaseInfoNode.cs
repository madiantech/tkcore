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
    public abstract class GroupBaseInfoNode : BaseInfoNode, IFileCreatorNode, IFileCreator, ISchemaData
    {
        private readonly static HashSet<string> IngoreKeys = new HashSet<string> { "TableOutput", "Item", "Add", "Override", "Except", "LevelTree" };
        private readonly XmlSchemaGroupBase fGroup;
        private readonly List<BaseNode> fChildren;
        private readonly bool fDynamic;

        public GroupBaseInfoNode(BaseNode parentNode, string caption, XmlSchemaGroupBase group, XmlSchemaSet schemaSet, HashSet<string> parents)
            : base(parentNode, caption)
        {
            fChildren = new List<BaseNode>();
            fGroup = group;
            foreach (var element in group.Items)
            {
                if (element is XmlSchemaElement elem)
                {
                    string name = elem.QualifiedName.Name;
                    if (!IngoreKeys.Contains(name) && parents.Contains(name))
                        fChildren.Add(new RecursionNode(this, name, elem.MinOccurs == 1));
                    else
                    {
                        ElementNode node = new ElementNode(this, elem, schemaSet, parents);
                        fChildren.Add(node);
                    }
                }
                else if (element is XmlSchemaSequence sequence)
                {
                    SequenceInfoNode node = new SequenceInfoNode(this, sequence, schemaSet, parents);
                    fChildren.Add(node);
                }
                else if (element is XmlSchemaChoice choice)
                {
                    ChoiceInfoNode node = new ChoiceInfoNode(this, choice, schemaSet, parents);
                    fChildren.Add(node);
                }
                else if (element is XmlSchemaAny any)
                {
                    if (!fDynamic)
                        fDynamic = any.Namespace == "##local";
                }
            }
        }

        protected List<BaseNode> ChildList { get => fChildren; }

        public override string Name => fDynamic ? base.Name + "$$$" : base.Name;

        public override IEnumerable<ITreeNode> Children
        {
            get
            {
                return fChildren;
            }
        }

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

        public void CreateRefContent(StringBuilder builder, string fileName, int level)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<BaseNode> GetChildNodes()
        {
            foreach (BaseNode node in fChildren)
            {
                if (node is ElementNode)
                    yield return (ElementNode)node;
                else if (node is GroupBaseInfoNode)
                {
                    IEnumerable<BaseNode> nodes = ((GroupBaseInfoNode)node).GetChildNodes();
                    foreach (var singleNode in nodes)
                        yield return singleNode;
                }
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