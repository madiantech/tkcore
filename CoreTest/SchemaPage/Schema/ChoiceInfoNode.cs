using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Toolkit.SchemaSuite.Schema
{
    public sealed class ChoiceInfoNode : GroupBaseInfoNode
    {
        private bool fIsRequired;
        private bool fIsMultiple;

        public ChoiceInfoNode(BaseNode node, XmlSchemaChoice choice, XmlSchemaSet schemaSet, HashSet<string> parents)
            : base(node, "选择", choice, schemaSet, parents)
        {
            fIsRequired = choice.MinOccurs == 1;
            fIsMultiple = choice.MaxOccursString == "unbounded";
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
                return fIsMultiple;
            }
        }
    }
}