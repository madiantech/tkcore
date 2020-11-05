using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal static class TreeUtil
    {
        private static string AddLayer(string value)
        {
            int nextValue = value.Value<int>() + 1;
            return nextValue.ToString(ObjectUtil.SysCulture).PadLeft(3, '0');
        }

        internal static void SetParentLeaf(TableResolver resolver, DbTreeDefinition fields, string parentId)
        {
            //DataTable table = resolver.HostTable;
            string sql = string.Format(ObjectUtil.SysCulture,
                "SELECT COUNT(*) FROM {0}", resolver.TableName);
            IParamBuilder builder = SqlParamBuilder.CreateEqualSql(
                resolver.Context, resolver.GetFieldInfo(fields.ParentIdField), parentId);
            builder = SqlParamBuilder.CreateParamBuilder(resolver.CreateFixCondition(), builder);
            int count = DbUtil.ExecuteScalar(sql, resolver.Context, builder).Value<int>();
            if (count == 1)
            {
                DataRow row = resolver.TrySelectRowWithParam(fields.IdField, parentId);
                if (row != null)
                    row[fields.LeafField] = 1;
            }
        }

        internal static IParamBuilder GetValueBuilder(TableSelector selector,
            DbTreeDefinition treeDef, int topLevel, NormalDataRowTreeNode node)
        {
            string layer = node.Layer;
            int len = layer.Length / 3;
            if (len < topLevel)
                return null;

            IParamBuilder[] builders = new IParamBuilder[len - topLevel + 1];
            for (int i = 0; i < builders.Length; i++)
            {
                int subLength = (topLevel + i - 1) * 3;
                string likeValue = layer.Substring(0, subLength).PadRight(subLength + 3, '_');
                string paramName = treeDef.LayerField + i.ToString(ObjectUtil.SysCulture);
                builders[i] = SqlParamBuilder.CreateSingleSql(selector.Context,
                    selector.GetFieldInfo(treeDef.LayerField), "LIKE", paramName, likeValue);
            }
            IParamBuilder builder = SqlParamBuilder.CreateParamBuilderWithOr(builders);
            return builder;
        }

        internal static IParamBuilder GetLevelBuilder(TableSelector selector,
            LevelTreeDefinition treeDef, int level, string value, ILevelProvider provider)
        {
            IParamBuilder[] builders = new IParamBuilder[level + 1];
            IFieldInfo idField = selector.GetFieldInfo(treeDef.IdField);
            for (int i = 0; i <= level; ++i)
            {
                string likeValue = provider.GetSqlLikeValue(treeDef, i, value);
                builders[i] = SqlParamBuilder.CreateSingleSql(selector.Context, idField, "LIKE",
                    treeDef.IdField + i, likeValue);
            }
            IParamBuilder builder = SqlParamBuilder.CreateParamBuilderWithOr(builders);
            return builder;
        }

        internal static ITreeNode GetTreeNode(TableSelector selector, ITreeNodeCreator creator, string id)
        {
            TkDebug.AssertArgumentNullOrEmpty(id, "id", creator);

            DataRow row = selector.TrySelectRowWithKeys(id);
            if (row == null)
                return null;
            return creator.CreateNode(row);
        }

        internal static IEnumerable<ITreeNode> SelectData(TableSelector selector, ITreeNodeCreator creator,
            Action action, string stateId)
        {
            DataTable table = selector.HostTable;
            int start = table == null ? 0 : table.Rows.Count;
            action();
            if (table == null)
                table = selector.HostTable;
            int end = table.Rows.Count;
            for (int i = start; i < end; i++)
            {
                DataRow row = table.Rows[i];
                DataRowTreeNode node = creator.CreateNode(row);
                if (node.Id == stateId)
                {
                    node.State = new TreeNodeState { Opened = true, Selected = true };
                }
                yield return node;
            }
        }

        internal static IEnumerable<ITreeNode> GetDisplayTreeNodes(IEnumerable<ITreeNode> data, string id)
        {
            RegNameList<DataRowTreeNode> list = new RegNameList<DataRowTreeNode>();
            foreach (DataRowTreeNode item in data)
                list.Add(item);

            DataRowTreeNode newNode = list[id];
            while (newNode != null)
            {
                newNode = list[newNode.ParentId];
                if (newNode != null)
                    newNode.HasChild = null;
            }

            return list;
        }

        private static IParamBuilder CreateLayerParamBuilder(TkDbContext context,
            IFieldInfo layer, string topLayer)
        {
            string fieldName = layer.FieldName;
            IParamBuilder builder = SqlParamBuilder.CreateParamBuilder(
                SqlParamBuilder.CreateSingleSql(context, layer, "!=", fieldName + "0", topLayer),
                SqlParamBuilder.CreateSingleSql(context, layer, "LIKE", fieldName + "1", topLayer + "%"));
            return builder;
        }

        internal static string GetLayer(TableResolver resolver, DbTreeDefinition tree, string parentId)
        {
            TkDbContext context = resolver.Context;
            IFieldInfo layerField = resolver.GetFieldInfo(tree.LayerField);
            IParamBuilder fixBuilder = resolver.CreateFixCondition();
            string execRootId = tree.ExecuteRootId;
            if (execRootId == parentId)
            {
                string subStringSql = context.ContextConfig.GetFunction("SubString",
                    layerField.FieldName, 1, 3);
                string sql = string.Format(ObjectUtil.SysCulture,
                    "SELECT MAX({0}) FROM {1}", subStringSql, resolver.TableName);
                string value = (fixBuilder == null ? DbUtil.ExecuteScalar(sql, context)
                    : DbUtil.ExecuteScalar(sql, context, fixBuilder)).ToString();
                if (string.IsNullOrEmpty(value))
                    return "000";
                else
                    return AddLayer(value);
            }
            else
            {
                try
                {
                    string sql = string.Format(ObjectUtil.SysCulture, "SELECT {0} FROM {1}",
                        layerField.FieldName, resolver.TableName);
                    IParamBuilder builder = ParamBuilder.CreateParamBuilder(fixBuilder,
                        SqlParamBuilder.CreateEqualSql(context, resolver.GetFieldInfo(tree.IdField), parentId));
                    string topLayer = DbUtil.ExecuteScalar(sql, context, builder).ToString();
                    string subStringSql = context.ContextConfig.GetFunction("SubString",
                        layerField.FieldName, topLayer.Length + 1, 3);

                    builder = CreateLayerParamBuilder(context, layerField, topLayer);
                    builder = SqlParamBuilder.CreateParamBuilder(fixBuilder, builder);
                    sql = string.Format(ObjectUtil.SysCulture, "SELECT MAX({0}) FROM {1}",
                        subStringSql, resolver.TableName);
                    string value = DbUtil.ExecuteScalar(sql, context, builder).ToString().Trim();
                    if (string.IsNullOrEmpty(value))
                    {
                        DataRow parentRow = resolver.SelectRowWithParam(tree.IdField, parentId);
                        parentRow[tree.LeafField] = 0;
                        resolver.SetCommands(AdapterCommand.Update);
                        return topLayer + "000";
                    }
                    else
                        return topLayer + AddLayer(value.Substring(value.Length - 3));
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        internal static void MoveTree(TableResolver resolver, DbTreeDefinition tree, string sourceId, string destId)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(tree, "tree", null);
            TkDebug.AssertArgumentNullOrEmpty(sourceId, "sourceId", null);
            TkDebug.AssertArgumentNullOrEmpty(destId, "destId", null);

            IFieldInfo layerField = resolver.GetFieldInfo(tree.LayerField);
            string sql = string.Format(ObjectUtil.SysCulture, "SELECT {0} FROM {1}",
                layerField.FieldName, resolver.TableName);
            IParamBuilder builder = ParamBuilder.CreateParamBuilder(resolver.CreateFixCondition(),
                SqlParamBuilder.CreateEqualSql(resolver.Context, resolver.GetFieldInfo(tree.IdField), destId));
            string destLayer = DbUtil.ExecuteScalar(sql, resolver.Context, builder).ToString();
            DataRow srcRow = resolver.SelectRowWithParam(tree.IdField, sourceId);
            if (destId == srcRow[tree.ParentIdField].ToString())
            {
                //throw new InvalidMoveException();
            }

            string oldLayer = srcRow[tree.LayerField].ToString();
            if (destLayer.StartsWith(oldLayer, StringComparison.Ordinal))
            {
                //throw new InvalidMoveException();
            }

            resolver.SetCommands(AdapterCommand.Update);

            string newLayer = GetLayer(resolver, tree, destId);
            srcRow[tree.LayerField] = newLayer;

            DataTable table = resolver.HostTable;
            int currentCount = table.Rows.Count;

            builder = CreateLayerParamBuilder(resolver.Context, layerField, oldLayer);
            resolver.Select(builder);
            for (int i = currentCount; i < table.Rows.Count; ++i)
            {
                DataRow row = table.Rows[i];
                row[tree.LayerField] = row[tree.LayerField].ToString().Replace(
                    oldLayer, newLayer);
            }

            SetParentLeaf(resolver, tree, srcRow[tree.ParentIdField].ToString());
            srcRow[tree.ParentIdField] = destId;
        }

        internal static void DeleteTree(TableResolver resolver, DbTreeDefinition tree, string id, IInputData inputData)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(tree, "tree", null);
            TkDebug.AssertArgumentNullOrEmpty(id, "id", null);

            DataRow row = null;
            if (resolver.HostTable != null)
                row = resolver.HostTable.Select(string.Format(ObjectUtil.SysCulture, "{0} = '{1}'",
                    tree.IdField, id))[0];
            else
                row = resolver.SelectRowWithParam(tree.IdField, id);

            resolver.SetCommands(AdapterCommand.Update | AdapterCommand.Delete);

            DataTable table = resolver.HostTable;
            int currentCount = table.Rows.Count;

            IFieldInfo layerField = resolver.GetFieldInfo(tree.LayerField);
            IParamBuilder builder = CreateLayerParamBuilder(resolver.Context, layerField,
                row[tree.LayerField].ToString());
            resolver.Select(builder);
            for (int i = currentCount; i < table.Rows.Count; ++i)
            {
                DataRow delRow = table.Rows[i];
                resolver.DeleteRow(delRow, UpdateKind.Delete, null, inputData);
            }

            SetParentLeaf(resolver, tree, row[tree.ParentIdField].ToString());
            resolver.DeleteRow(row, UpdateKind.Delete, null, inputData);
        }

        private static int GetRowNum(DataTable table, string keyFieldName, string id)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][keyFieldName].ToString() == id)
                    return i;
            }
            return -1;
        }

        private static void ChangeChildLayer(TableResolver resolver,
            DbTreeDefinition fields, string id, string layer, int isLeaf)
        {
            if (isLeaf == 0)
            {
                string layerField = fields.LayerField;
                string parentField = fields.ParentIdField;
                resolver.SelectWithParam(parentField, id);

                DataRow[] childRows = resolver.HostTable.Select(string.Format(
                    ObjectUtil.SysCulture, "{0} = {1}", parentField, id), layerField + " ASC");

                for (int i = 0; i < childRows.Length; i++)
                {
                    DataRow childRow = childRows[i];
                    string childLayer = childRow[layerField].ToString();
                    string lastThree = childLayer.Substring(childLayer.Length - 3, 3);
                    childRows[i][layerField] = string.Format(ObjectUtil.SysCulture,
                        "{0}{1}", layer, lastThree);
                    ChangeChildLayer(resolver, fields, childRow[fields.IdField].ToString(),
                        childRow[fields.LayerField].ToString(), childRow[fields.LeafField].Value<int>());
                }
            }
        }

        private static void SwapLayer(TableResolver resolver, DbTreeDefinition fields,
            int rowNum1, int rowNum2)
        {
            string layerField = fields.LayerField;
            string leafField = fields.LeafField;

            DataRow row1 = resolver.HostTable.Rows[rowNum1];
            DataRow row2 = resolver.HostTable.Rows[rowNum2];
            string tempLayer = row1[layerField].ToString();
            row1[layerField] = row2[layerField];
            row2[layerField] = tempLayer;
            ChangeChildLayer(resolver, fields, row1[fields.IdField].ToString(),
                row1[layerField].ToString(), row1[leafField].Value<int>());
            ChangeChildLayer(resolver, fields, row2[fields.IdField].ToString(),
                row2[layerField].ToString(), row2[leafField].Value<int>());
        }

        internal static void SortTree(TableResolver resolver, DbTreeDefinition tree,
            string id, TreeNodeMoveDirection direct)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(tree, "tree", null);
            TkDebug.AssertArgumentNullOrEmpty(id, "id", null);

            IParamBuilder fixBuilder = resolver.CreateFixCondition();
            IParamBuilder builder = ParamBuilder.CreateParamBuilder(fixBuilder,
                SqlParamBuilder.CreateEqualSql(resolver.Context, resolver.GetFieldInfo(tree.IdField), id));

            IFieldInfo layerField = resolver.GetFieldInfo(tree.LayerField);
            string sql = string.Format(ObjectUtil.SysCulture, "SELECT {0} FROM {1}",
                layerField.FieldName, resolver.TableName);
            string layer = DbUtil.ExecuteScalar(sql, resolver.Context, builder).ToString();
            string parentLayer = layer.Substring(0, layer.Length - 3);

            resolver.SetCommands(AdapterCommand.Update);
            builder = SqlParamBuilder.CreateSingleSql(resolver.Context, layerField, "LIKE", parentLayer + "___");
            resolver.Select(builder, "ORDER BY " + layerField.FieldName);
            if (resolver.HostTable == null || resolver.HostTable.Rows.Count == 0)
                return;

            int rowNum = GetRowNum(resolver.HostTable, tree.IdField, id);
            if (rowNum == -1)
                return;
            //根据移动方向，执行不同操作
            switch (direct)
            {
                case TreeNodeMoveDirection.Up:
                    if (rowNum == 0)//已经最前，不能向上移动
                        return;
                    SwapLayer(resolver, tree, rowNum, rowNum - 1);
                    break;

                case TreeNodeMoveDirection.Down:
                    if (rowNum == resolver.HostTable.Rows.Count - 1)//已经最后，不能向下移动
                        return;
                    SwapLayer(resolver, tree, rowNum, rowNum + 1);
                    break;
            }
        }
    }
}