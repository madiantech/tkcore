using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using YJC.Toolkit.Data;

namespace Toolkit.SchemaSuite.Schema
{
    public sealed class AttributeInfoNode : BaseInfoNode, IFileCreatorNode
    {
        private List<AttributeNode> fChildren;

        public AttributeInfoNode(ElementNode parentNode, XmlSchemaObjectTable attributes)
            : base(parentNode, "属性")
        {
            fChildren = new List<AttributeNode>(attributes.Count);
            foreach (XmlSchemaAttribute attr in attributes.Values)
            {
                AttributeNode node = new AttributeNode(this, parentNode, attr);
                fChildren.Add(node);
            }
        }

        public override IEnumerable<ITreeNode> Children
        {
            get
            {
                return fChildren;
            }
        }

        public void CreateContent(StringBuilder builder, int level, bool isRoot)
        {
            foreach (var node in fChildren)
                node.CreateContent(builder, level, false);
        }

        public void CreateRefContent(StringBuilder builder, string fileName, int level)
        {
            throw new NotImplementedException();
        }

        public bool IsOrderAttribute
        {
            get
            {
                if (fChildren.Count == 1 && fChildren[0].RealName == SchemaConst.ORDER)
                    return true;

                return false;
            }
        }
    }
}