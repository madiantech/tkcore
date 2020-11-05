using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Toolkit.SchemaSuite.Schema
{
    public sealed class RootNode : ElementNode
    {
        public RootNode(XmlSchemaElement root, XmlSchemaSet schemaSet, string schemaFile)
            : base(null, root, schemaSet, null)
        {
        }

        public override bool IsMultiple
        {
            get
            {
                return false;
            }
        }

        public override bool IsRequired
        {
            get
            {
                return true;
            }
        }
    }
}