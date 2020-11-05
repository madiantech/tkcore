using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class NormalDbTree : ITree, IDisposable, ITreeNodeCreator, ITreeOperationCreator, IFieldInfoIndexer
    {
        private bool fInitRoot;
        private IEnumerable<ITreeNode> fRootNodes;
        private string fRootParentId;
        private int fTopLevel;
        private readonly IDbDataSource fSource;
        private readonly ITableScheme fScheme;
        private readonly DbTreeDefinition fDefinition;
        private readonly TreeSelector fSelector;
        private readonly string fOrder;
        private readonly ITreeOperation fTreeOperation;

        public NormalDbTree(ITableScheme scheme, DbTreeDefinition treeDef, IDbDataSource source)
            : this(scheme, treeDef, source, null)
        {
            fTreeOperation = new DefaultTreeOperation(TreeOperation.NewChild, treeDef);
        }

        internal NormalDbTree(ITableScheme scheme, DbTreeDefinition treeDef, IDbDataSource source, ITreeOperation operation)
        {
            TkDebug.AssertArgumentNull(source, "source", null);
            TkDebug.AssertArgumentNull(scheme, "scheme", null);
            TkDebug.AssertArgumentNull(treeDef, "treeDef", null);

            fSource = source;
            fScheme = scheme;
            fDefinition = treeDef;
            fOrder = "ORDER BY " + fScheme[treeDef.LayerField].FieldName;
            fSelector = new TreeSelector(scheme, treeDef, source);
            fTreeOperation = operation;
        }

        #region ITree 成员

        public int TopLevel
        {
            get
            {
                InitTree();
                return fTopLevel;
            }
        }

        public string RootParentId
        {
            get
            {
                InitTree();
                return fRootParentId;
            }
        }

        public IEnumerable<ITreeNode> RootNodes
        {
            get
            {
                InitTree();
                return fRootNodes;
            }
        }

        private IParamBuilder GetCustomParamBuilder(IParamBuilder builder)
        {
            IParamBuilder result = builder;
            if (DataRight != null)
            {
                TkDebug.ThrowIfNoGlobalVariable();
                IUserInfo userInfo = BaseGlobalVariable.Current.UserInfo;
                IParamBuilder dataRight = DataRight.GetListSql(
                    new ListDataRightEventArgs(fSelector.Context, userInfo, fSelector));
                result = ParamBuilder.CreateParamBuilder(result, dataRight);
            }
            if (CustomCondition != null)
                result = ParamBuilder.CreateParamBuilder(result, CustomCondition);

            return result;
        }

        public ITreeNode GetTreeNode(string id)
        {
            if (!UseCustomParamBuilder)
                return TreeUtil.GetTreeNode(fSelector, this, id);
            else
            {
                IParamBuilder builder = fSelector.CreateParamBuilder(null,
                    new string[] { fDefinition.IdField }, id);
                return GetTreeNodes(builder).FirstOrDefault();
            }
        }

        public IEnumerable<ITreeNode> GetDisplayTreeNodes(string id)
        {
            ITreeNode node = GetTreeNode(id);
            if (node == null)
                return RootNodes;

            IParamBuilder builder = TreeUtil.GetValueBuilder(fSelector, fDefinition,
                TopLevel, node.Convert<NormalDataRowTreeNode>());
            builder = GetCustomParamBuilder(builder);
            if (builder == null)
                return Enumerable.Empty<ITreeNode>();
            //IParamBuilder customSql = fTreeProvider.CustomSql;
            //if (builder == null && customSql == null)
            //    return TreeUtil.CreateEmptyEnumerable();
            //if (customSql != null)
            //    builder = builder == null ? customSql : SqlParamBuilder.CreateParamBuilder(builder, customSql);
            var data = TreeUtil.SelectData(fSelector, this, () => fSelector.Select(builder, fOrder), id);
            return TreeUtil.GetDisplayTreeNodes(data, id);
        }

        public IEnumerable<ITreeNode> GetChildNodes(string parentId)
        {
            TkDebug.AssertArgumentNullOrEmpty(parentId, "parentId", this);

            if (!UseCustomParamBuilder)
                return TreeUtil.SelectData(fSelector, this,
                    () => fSelector.SelectWithParam(null, fOrder, fDefinition.ParentIdField, parentId),
                    null).ToArray();
            else
            {
                IParamBuilder builder = fSelector.CreateParamBuilder(null,
                    new string[] { fDefinition.ParentIdField }, parentId);
                return GetTreeNodes(builder).ToArray();
            }
        }

        #endregion ITree 成员

        #region IDisposable 成员

        public void Dispose()
        {
            fSelector.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        #region ITreeNodeCreator 成员

        public DataRowTreeNode CreateNode(DataRow row)
        {
            return new NormalDataRowTreeNode(this, row, fDefinition);
        }

        #endregion ITreeNodeCreator 成员

        #region ITreeOperationCreator 成员

        public ITreeOperation CreateOperation()
        {
            return fTreeOperation;
        }

        #endregion ITreeOperationCreator 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                return fSelector[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        public IDataRight DataRight { get; set; }

        public IParamBuilder CustomCondition { get; set; }

        private bool UseCustomParamBuilder
        {
            get
            {
                return DataRight != null || CustomCondition != null;
            }
        }

        private IEnumerable<ITreeNode> GetTreeNodes(IParamBuilder builder)
        {
            builder = GetCustomParamBuilder(builder);
            return TreeUtil.SelectData(fSelector, this,
                () => fSelector.Select(builder, fOrder), null);
        }

        private void ProcessIdRootNode(string rootId)
        {
            DataRow row;
            if (!UseCustomParamBuilder)
                row = fSelector.TrySelectRowWithKeys(rootId);
            else
            {
                IParamBuilder builder = fSelector.CreateParamBuilder(null,
                    new string[] { fDefinition.IdField }, rootId);
                builder = GetCustomParamBuilder(builder);
                row = fSelector.TrySelectRow(builder);
            }
            if (row != null)
            {
                fRootParentId = row[fDefinition.ParentIdField].ToString();
            }
            else
            {
                fRootParentId = DbTreeDefinition.DEFAULT_PARENT_VALUE;
                fRootNodes = Enumerable.Empty<ITreeNode>();
                SetTopLevel(null);
            }
            if (row != null)
            {
                // 由于在CreateNode的时候需要检测RootId，所以必须先给RootParentId赋值，然后才能创建Node
                ITreeNode root = CreateNode(row);
                fRootNodes = EnumUtil.Convert(root);
                SetTopLevel(root);
            }
        }

        private void ProcessCustomRootNodes(string rootId)
        {
            IParamBuilder builder = string.IsNullOrEmpty(rootId) ? null
                : ParamBuilder.CreateSql(rootId);
            if (UseCustomParamBuilder)
                builder = GetCustomParamBuilder(builder);

            DataTable table = fSelector.HostTable;
            int start = table == null ? 0 : table.Rows.Count;
            fSelector.Select(builder);
            if (table == null)
                table = fSelector.HostTable;
            int end = table.Rows.Count;

            // 没有获得数据
            if (start == end)
            {
                fRootParentId = DbTreeDefinition.DEFAULT_PARENT_VALUE;
                fRootNodes = Enumerable.Empty<ITreeNode>();
                SetTopLevel(null);
                return;
            }

            // 获得数据
            DataRow row = table.Rows[start];
            fRootParentId = row[fDefinition.ParentIdField].ToString();
            fRootNodes = GetRootNodes(table, start, end);
            SetTopLevel(fRootNodes.First());
        }

        private IEnumerable<ITreeNode> GetRootNodes(DataTable table, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                DataRow row = table.Rows[i];
                DataRowTreeNode node = CreateNode(row);
                yield return node;
            }
        }

        private void InitTree()
        {
            if (!fInitRoot)
            {
                fInitRoot = true;
                string rootId = fDefinition.ExecuteRootId;
                switch (fDefinition.SearchType)
                {
                    case RootSearchType.Id:
                        ProcessIdRootNode(rootId);
                        break;

                    case RootSearchType.ParentId:
                        fRootParentId = rootId;
                        fRootNodes = GetChildNodes(fRootParentId);
                        SetTopLevel(fRootNodes.FirstOrDefault());
                        break;

                    case RootSearchType.Custom:
                        ProcessCustomRootNodes(rootId);
                        break;
                }
            }
        }

        private void SetTopLevel(ITreeNode node)
        {
            fTopLevel = (node == null) ? 0 : node.Convert<NormalDataRowTreeNode>().Layer.Length / 3;
        }
    }
}