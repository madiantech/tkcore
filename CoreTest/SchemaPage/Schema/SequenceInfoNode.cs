using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Toolkit.SchemaSuite.Schema
{
    public sealed class SequenceInfoNode : GroupBaseInfoNode, IJudgeNode
    {
        private bool fIsRequired;
        private bool fIsMultiple;

        public SequenceInfoNode(BaseNode parentNode, XmlSchemaSequence sequence, XmlSchemaSet schemaSet, HashSet<string> parents)
            : base(parentNode, "序列", sequence, schemaSet, parents)
        {
            fIsRequired = sequence.MinOccurs == 1;
            fIsMultiple = sequence.MaxOccursString == "unbounded";
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

        public override string Name
        {
            get
            {
                if (IsMultiple && IsRequired)
                    return base.Name + "(+)";
                else if (IsMultiple && !IsMultiple)
                    return base.Name + "(*)";
                else
                    return base.Name;
            }
        }

        public NodeType NodeType { get; private set; }

        public void Execute(int level)
        {
            // 判断附加
            if (ChildList.Count == 1 && ChildList[0] is ElementNode element)
            {
                if (element.RealName == SchemaConst.CHECK_BOX)
                {
                    NodeType = NodeType.CheckBox;
                    return;
                }
            }
            // 判断日期
            if (ChildList.Count == 3)
            {
                if ((ChildList[0] is ElementNode year) && (ChildList[1] is ElementNode month) && (ChildList[2] is ElementNode day))
                {
                    if (year.RealName == SchemaConst.YEAR && month.RealName == SchemaConst.MONTH && day.RealName == SchemaConst.DAY)
                    {
                        NodeType = NodeType.Date;
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
    }
}