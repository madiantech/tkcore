using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Toolkit.SchemaSuite.Schema;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    [DebuggerDisplay("{NickName}")]
    public class SchemaField : IFieldInfo, IFieldInfoEx, IRegName
    {
        private readonly static IFieldLayout fLayout = SimpleFieldLayout.CreateDefault();
        private readonly InternalFieldControl fControl;
        private readonly string fName;
        private readonly string fFieldName;
        private readonly bool fIsEmpty;
        private readonly TkDataType fDataType;

        public SchemaField(ElementNode node)
        {
            fName = node.RealName;
            fIsEmpty = !node.IsRequired;
            ControlType ctrl = ControlType.Text;
            fFieldName = GetParentPath(node) + node.RealName;
            switch (node.NodeType)
            {
                case NodeType.CheckBox:
                    ctrl = ControlType.CheckBox;
                    fFieldName += ".附加";
                    break;

                case NodeType.Date:
                    ctrl = ControlType.Date;
                    break;
            }
            fControl = new InternalFieldControl { Control = ctrl };
        }

        public SchemaField(AttributeNode node)
        {
            fName = node.RealName;
            fIsEmpty = !node.IsRequired;
            fControl = new InternalFieldControl { Control = ControlType.Text };
            fFieldName = GetParentPath(node) + node.RealName;
        }

        [SimpleElement]
        public string FieldName { get => fFieldName; }

        [SimpleElement]
        public string DisplayName { get => fName; }

        [SimpleElement]
        public string NickName { get => fFieldName; }

        [SimpleElement]
        public TkDataType DataType { get => fDataType; }

        [SimpleElement]
        public bool IsKey { get => false; }

        [SimpleElement]
        public bool IsAutoInc { get => false; }

        [SimpleElement]
        public int Length { get => 256; }

        [SimpleElement]
        public bool IsEmpty { get => fIsEmpty; }

        public int Precision { get => 0; }

        [SimpleElement]
        public FieldKind Kind { get => FieldKind.Data; }

        public string Expression { get => null; }

        [ObjectElement(ObjectType = typeof(SimpleFieldLayout))]
        public IFieldLayout Layout { get => fLayout; }

        [ObjectElement(ObjectType = typeof(InternalFieldControl))]
        public IFieldControl Control { get => fControl; }

        public IFieldDecoder Decoder { get => null; }

        public IFieldUpload Upload { get => null; }

        public string RegName { get => NickName; }

        public bool IsShowInList(IPageStyle style, bool isInTable) => true;

        public override string ToString() => NickName;

        private string GetParentPath(BaseNode node)
        {
            List<ElementNode> list = new List<ElementNode>();
            var parentNode = node.Parent;
            while (parentNode != null)
            {
                if (parentNode is ElementNode eleNode)
                    list.Add(eleNode);
                parentNode = parentNode.Parent;
            }
            if (list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in list)
                    builder.Append(item.RealName).Append(".");
                return builder.ToString();
            }
            else
                return string.Empty;
        }

        public ControlType ControlType { get => fControl.Control; }

        public int Order { get => fControl.Order; }

        public void SetOrder(int order)
        {
            fControl.Order = order;
        }
    }
}