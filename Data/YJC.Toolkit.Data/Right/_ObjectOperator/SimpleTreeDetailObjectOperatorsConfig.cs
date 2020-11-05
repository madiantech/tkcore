using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [ObjectOperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-11-29", Description = "默认树详细页面对象操作符配置")]
    internal class SimpleTreeDetailObjectOperatorsConfig : IConfigCreator<IObjectOperatorsConfig>,
        IObjectOperatorsConfig, IReadObjectCallBack, IConfigCreator<IObjectOperateRight>
    {
        private readonly LinkedList<OperatorConfig> fOperatorList;
        private readonly LinkedListNode<OperatorConfig> fUpdateNode;
        private readonly LinkedListNode<OperatorConfig> fDeleteNode;
        private readonly LinkedListNode<OperatorConfig> fInsertNode;
        private TreeObjectOperateRight fRight;

        public SimpleTreeDetailObjectOperatorsConfig()
        {
            fUpdateNode = new LinkedListNode<OperatorConfig>(new OperatorConfig(RightConst.UPDATE,
                "修改节点", OperatorPosition.Global, RightConst.UPDATE_DIALOG, null, "icon-edit", null));
            fDeleteNode = new LinkedListNode<OperatorConfig>(new OperatorConfig(RightConst.DELETE,
                "删除节点", OperatorPosition.Global, RightConst.DELETE + ",AjaxUrl", "确定删除吗？",
                "icon-remove", null));
            fInsertNode = new LinkedListNode<OperatorConfig>(new OperatorConfig(RightConst.INSERT,
                "新建子节点", OperatorPosition.Global, "Dialog", null, "icon-plus",
                new MarcoConfigItem(true, true, "~/c/~xml/CNewChild/{CcSource}"))
            { UseKey = true });

            fOperatorList = new LinkedList<OperatorConfig>();
            fOperatorList.AddLast(fUpdateNode);
            fOperatorList.AddLast(fDeleteNode);
            fOperatorList.AddLast(fInsertNode);
        }

        #region IObjectOperatorsConfig 成员

        public IConfigCreator<IObjectOperateRight> Right
        {
            get
            {
                return this;
            }
        }

        public IEnumerable<OperatorConfig> Operators
        {
            get
            {
                return fOperatorList;
            }
        }

        #endregion IObjectOperatorsConfig 成员

        #region IConfigCreator<IObjectOperatorsConfig> 成员

        public IObjectOperatorsConfig CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IObjectOperatorsConfig> 成员

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    item.CreateOperator();
                    LinkedListNode<OperatorConfig> node = null;
                    switch (item.Button)
                    {
                        case UpdateKind.Insert:
                            node = fInsertNode;
                            break;

                        case UpdateKind.Update:
                            node = fUpdateNode;
                            break;

                        case UpdateKind.Delete:
                            node = fDeleteNode;
                            break;
                    }
                    switch (item.Position)
                    {
                        case TreeOperatorPosition.Before:
                            fOperatorList.AddBefore(node, item.OperatorConfig);
                            break;

                        case TreeOperatorPosition.After:
                            fOperatorList.AddAfter(node, item.OperatorConfig);
                            break;
                    }
                }
            }

            if (!IsDialog)
            {
                fUpdateNode.Value.Info = RightConst.UPDATE;
                fInsertNode.Value.Info = string.Empty;
            }
        }

        #endregion IReadObjectCallBack 成员

        #region IConfigCreator<IObjectOperateRight> 成员

        IObjectOperateRight IConfigCreator<IObjectOperateRight>.CreateObject(params object[] args)
        {
            if (fRight == null)
            {
                if (Items == null)
                    fRight = new TreeObjectOperateRight();
                else
                {
                    var operatorIds = from item in Items
                                      select item.OperatorConfig.Id;
                    fRight = new TreeObjectOperateRight(operatorIds);
                }
                fRight.DisableRootDelete = DisableRootDelete;
            }
            return fRight;
        }

        #endregion IConfigCreator<IObjectOperateRight> 成员

        [SimpleAttribute(DefaultValue = true)]
        public bool DisableRootDelete { get; private set; }

        [ObjectElement(NamespaceType = NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item")]
        public List<TreeDetailOperator> Items { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool IsDialog { get; set; }
    }
}