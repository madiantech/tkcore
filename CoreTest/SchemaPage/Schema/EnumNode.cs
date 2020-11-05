using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Toolkit.SchemaSuite.Schema
{
    public sealed class EnumNode : BaseInfoNode
    {
        private readonly List<string> fItems;
        private string fTypeName;

        public EnumNode(BaseNode parentNode, string typeName, XmlSchemaSimpleTypeRestriction enumContent)
            : base(parentNode, "枚举")
        {
            fTypeName = typeName;
            fItems = new List<string>(enumContent.Facets.Count);
            foreach (XmlSchemaEnumerationFacet item in enumContent.Facets)
                fItems.Add(item.Value);
        }

        public List<string> Items
        {
            get
            {
                return fItems;
            }
        }

        public override bool HasChildren
        {
            get
            {
                return false;
            }
        }
    }
}