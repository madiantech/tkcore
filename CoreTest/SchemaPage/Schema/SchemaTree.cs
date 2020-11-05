using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite.Schema
{
    public class SchemaTree : ITree, IFileCreator
    {
        private static readonly XmlQualifiedName ROOT_NODE = new XmlQualifiedName(ToolkitConst.TOOLKIT);
        private static readonly XmlQualifiedName ROOT_NODE2 = new XmlQualifiedName(ToolkitConst.TOOLKIT, ToolkitConst.NAMESPACE_URL);
        private readonly RootNode fRootNode;
        private readonly string fFileName;

        public SchemaTree(string schemaFile, string rootNamespace, string rootNode)
        {
            //bool useTk = false;
            //XmlSchemaSet set;
            //try
            //{
            //    set = new XmlSchemaSet();
            //    set.Add(string.Empty, schemaFile);
            //}
            //catch (XmlSchemaException)
            //{
            //    useTk = true;
            //    set = new XmlSchemaSet();
            //    set.Add(ToolkitConst.NAMESPACE_URL, schemaFile);
            //}
            //set.Compile();
            //XmlSchemaObject root = set.GlobalElements[useTk ? ROOT_NODE2 : ROOT_NODE];
            //if (root != null)
            //    fRootNode = new RootNode(root as XmlSchemaElement, schemaFile);
            //else
            //    fRootNode = null;
            fFileName = Path.GetFileNameWithoutExtension(schemaFile);
            string ns;
            XmlQualifiedName rootName;
            if (string.IsNullOrEmpty(rootNamespace))
            {
                ns = string.Empty;
                rootName = new XmlQualifiedName(rootNode);
            }
            else
            {
                ns = rootNamespace;
                rootName = new XmlQualifiedName(rootNode, rootNamespace);
            }
            XmlSchemaObject root = null;
            XmlSchemaSet set = new XmlSchemaSet();
            try
            {
                set.XmlResolver = new XmlUrlResolver();
                set.Add(ns, schemaFile);
                set.Compile();
                root = set.GlobalElements[rootName];
            }
            catch (Exception ex)
            {
            }
            if (root != null)
                fRootNode = new RootNode(root as XmlSchemaElement, set, schemaFile);
            else
                fRootNode = null;
        }

        #region ITree 成员

        public IEnumerable<ITreeNode> GetChildNodes(string parentId)
        {
            return null;
        }

        public IEnumerable<ITreeNode> GetDisplayTreeNodes(string id)
        {
            return null;
        }

        public ITreeNode GetTreeNode(string id)
        {
            return null;
        }

        public IEnumerable<ITreeNode> RootNodes
        {
            get
            {
                if (fRootNode != null)
                    return EnumUtil.Convert(fRootNode);
                else
                    return null;
            }
        }

        public string RootParentId
        {
            get
            {
                return "-1";
            }
        }

        public int TopLevel
        {
            get
            {
                return 1;
            }
        }

        #endregion ITree 成员

        private static void InternalCreateFile()
        {
            //if (nodes == null)
            //    return;

            //foreach (BaseNode node in nodes)
            //{
            //    if (node.IsChecked == false)
            //        continue;
            //    IFileCreatorNode creatorNode = node as IFileCreatorNode;
            //    if (creatorNode != null)
            //    {
            //        string typeName = creatorNode.TypeName;
            //        if (creator.RegTypes != null && creator.RegTypes.Contains(typeName))
            //            continue;
            //        if (!types.Contains(typeName))
            //        {
            //            types.Add(typeName);
            //            creatorNode.CreateFile(schemaData, creator);
            //        }
            //    }

            //    if (node.HasChildren)
            //        InternalCreateFile(types, schemaData, creator, node.Children);
            //}
        }

        public void CreateFile()
        {
        }

        private void WriteNode(ITreeNode node, int level)
        {
            string space = " ".PadLeft(level * 2, ' ');
            Console.WriteLine($"{space}{node.Name}");
            if (node.HasChildren)
            {
                foreach (var childNode in node.Children)
                    WriteNode(childNode, level + 1);
            }
        }

        public void WriteTree()
        {
            foreach (var rootNode in RootNodes)
            {
                WriteNode(rootNode, 0);
            }
        }

        public void CreateMdFile(string path)
        {
            StringBuilder builder = new StringBuilder();

            SchemaSuiteUtil.AppendLineFrontMatter(builder);

            fRootNode.CreateContent(builder, 0, true);
            string fileName = Path.Combine(path, $"{fFileName}.md");
            FileUtil.VerifySaveFile(fileName, builder.ToString(), Encoding.UTF8);

            InternalCreateMdFile(RootNodes, path);
        }

        private void InternalCreateMdFile(IEnumerable<ITreeNode> nodes, string path)
        {
            foreach (var node in nodes)
            {
                if (node is IFileCreator creator)
                {
                    creator.CreateMdFile(path);
                }
                if (node.HasChildren)
                {
                    InternalCreateMdFile(node.Children, path);
                }
            }
        }

        private void InternalJudge(IEnumerable<ITreeNode> nodes, int level)
        {
            foreach (var node in nodes)
                if (node is IJudgeNode judge)
                    judge.Execute(level);
        }

        public void Judge()
        {
            InternalJudge(RootNodes, 1);
        }

        public SchemaTable CreateMetaData(PageData pageData)
        {
            SchemaTable table = new SchemaTable(fRootNode.RealName);
            fRootNode.AddMetaData(table, pageData);

            return table;
        }
    }
}