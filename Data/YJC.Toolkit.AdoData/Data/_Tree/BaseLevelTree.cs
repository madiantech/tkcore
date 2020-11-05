using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public abstract class BaseLevelTree : ITree, IDisposable, ITreeNodeCreator, ITreeOperationCreator
    {
        private static readonly ITreeOperation Operation =
            new DefaultTreeOperation(TreeOperation.Base, string.Empty);

        private readonly LevelSelector fSelector;
        private readonly ILevelProvider fProvider;
        private readonly LevelTreeDefinition fDefinition;

        internal BaseLevelTree(ITableScheme scheme, LevelTreeDefinition levelDef,
            IDbDataSource source, ILevelProvider provider)
        {
            fSelector = new LevelSelector(scheme, levelDef, source);
            fProvider = provider;
            fDefinition = levelDef;
        }

        #region ITree 成员

        public int TopLevel
        {
            get
            {
                return 1;
            }
        }

        public string RootParentId
        {
            get
            {
                return "#";
            }
        }

        public IEnumerable<ITreeNode> RootNodes
        {
            get
            {
                return SelectChildNode(null, 0);
            }
        }

        public ITreeNode GetTreeNode(string id)
        {
            return TreeUtil.GetTreeNode(fSelector, this, id);
        }

        public IEnumerable<ITreeNode> GetDisplayTreeNodes(string id)
        {
            int level = fProvider.GetLevel(fDefinition, id);
            IParamBuilder builder = TreeUtil.GetLevelBuilder(fSelector, fDefinition,
                level, id, fProvider);

            var data = TreeUtil.SelectData(fSelector, this, () => fSelector.Select(builder), id);
            return TreeUtil.GetDisplayTreeNodes(data, id);
        }

        public IEnumerable<ITreeNode> GetChildNodes(string parentId)
        {
            int level = fProvider.GetLevel(fDefinition, parentId);
            if (level == fDefinition.TotalLevel - 1)
                return Enumerable.Empty<ITreeNode>();
            else
                return SelectChildNode(parentId, level + 1);
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ITreeNodeCreator 成员

        public DataRowTreeNode CreateNode(DataRow row)
        {
            return new LevelDataRowTreeNode(this, row, fDefinition, fProvider);
        }

        #endregion

        #region ITreeOperationCreator 成员

        public virtual ITreeOperation CreateOperation()
        {
            return Operation;
        }

        #endregion

        private IEnumerable<ITreeNode> SelectChildNode(string parentId, int level)
        {
            string value = fProvider.GetSqlLikeValue(fDefinition, level, parentId);
            IFieldInfo fieldInfo = fSelector.GetFieldInfo(fDefinition.IdField);
            IParamBuilder builder = SqlParamBuilder.CreateSingleSql(fSelector.Context,
                fieldInfo, "LIKE", value);
            if (!string.IsNullOrEmpty(parentId))
                builder = SqlParamBuilder.CreateParamBuilder(builder,
                   SqlParamBuilder.CreateSingleSql(fSelector.Context, fieldInfo,
                   "<>", fDefinition.IdField + 1, parentId));
            return TreeUtil.SelectData(fSelector, this, () => fSelector.Select(builder), null).ToArray();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                fSelector.Dispose();
        }
    }
}
