using System;
using System.Text;
using System.Xml.Schema;

namespace Toolkit.SchemaSuite.Schema
{
    public sealed class RecursionNode : BaseInfoNode, IFileCreatorNode
    {
        private readonly string fName;
        private readonly bool fIsRequired;

        public RecursionNode(BaseNode parentNode, string name, bool required)
            : base(parentNode, "...")
        {
            fName = name;
            fIsRequired = required;
        }

        public override bool HasChildren
        {
            get
            {
                return false;
            }
        }

        public void CreateContent(StringBuilder builder, int level, bool isRoot)
        {
            string title = string.Empty.PadLeft(level + 1, '#');
            builder.Append($"{title} {fName}").AppendLine();
            builder.AppendFormat("- 必须: {0}", fIsRequired ? "是" : "否").AppendLine();
            builder.Append($"- 说明: 递归节点，调用了父节点的内容，不再继续显示").AppendLine();
        }

        public void CreateRefContent(StringBuilder builder, string fileName, int level)
        {
        }
    }
}