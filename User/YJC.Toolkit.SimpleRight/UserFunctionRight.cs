using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    internal class UserFunctionRight : EmptyFunctionRight, IDisposable
    {
        private const string FUNC_SQL = "SELECT DISTINCT FN_ID, FN_SHORT_NAME, FN_NAME, FN_URL,"
            + " FN_PARENT_ID, FN_TREE_LAYER, FN_IS_LEAF, ({1} / 3 - 1) AS FN_LAYER "
            + "FROM SYS_FUNCTION WHERE FN_ID IN (SELECT PF_FN_ID FROM SYS_PART_FUNC WHERE PF_PART_ID IN "
            + "(SELECT UP_PART_ID FROM UR_USERS_PART WHERE UP_USER_ID = {0})) ORDER BY FN_LAYER, FN_TREE_LAYER";

        private const string SUB_FUNC_SQL = "SELECT SF_ID Id, SF_FN_ID FnId, SF_NAME_ID NameId, "
            + "SF_NAME Name, SF_POSITION Position, SF_ICON Icon, SF_USE_KEY UseKey, "
            + "SF_CONTENT Content, SF_USE_MARCO UseMarco, SF_CONFIRM_DATA ConfirmData, "
            + "SF_DIALOG_TITLE DialogTitle, SF_INFO Info, SF_PAGE Page, SF_ORDER OperOrder "
            + "FROM SYS_SUB_FUNC WHERE SF_ID IN (SELECT PSF_SF_ID FROM SYS_PART_SUB_FUNC "
            + "WHERE PSF_PART_ID IN (SELECT UP_PART_ID FROM UR_USERS_PART WHERE UP_USER_ID = {0})) "
            + "ORDER BY SF_FN_ID, SF_ID";

        private readonly Dictionary<string, FunctionItem> fFunctions;
        private DataSet fDataSet;
        private bool fAdmin;

        public UserFunctionRight()
        {
            fFunctions = new Dictionary<string, FunctionItem>();
        }

        public override void Initialize(IUserInfo data)
        {
            if (data != null)
            {
                fFunctions.Clear();
                SetDataSet(data.UserId);
            }
        }

        public override bool IsAdmin()
        {
            return fAdmin;
        }

        public override bool IsFunction(object key)
        {
            if (key == null)
                return false;
            FunctionItem item;
            if (fFunctions.TryGetValue(key.ToString(), out item))
                return item.IsLeaf;
            return false;
        }

        public override bool IsSubFunction(SubFunctionKey subKey, object key)
        {
            if (key == null || subKey == null)
                return false;
            FunctionItem item;
            if (fFunctions.TryGetValue(key.ToString(), out item))
                return item.IsSubFunction(subKey);
            return false;
        }

        public IEnumerable<string> GetSubFunctions(object key)
        {
            if (key == null)
                return null;
            FunctionItem item;
            if (fFunctions.TryGetValue(key.ToString(), out item))
                return item.SubFunctions;

            return null;
        }

        public override DataSet GetMenuObject(object userId)
        {
            return fDataSet.Copy();
        }

        private void SetDataSet(object userId)
        {
            fDataSet = new DataSet { Locale = CultureInfo.CurrentCulture };
            TkDbContext context = DbContextUtil.CreateDefault();
            using (context)
            {
                IParamBuilder builder = SqlParamBuilder.CreateEqualSql(context, "USER_ID", TkDataType.String, userId);
                fAdmin = DbUtil.ExecuteScalar("SELECT USER_ADMIN FROM UR_USERS", context, builder).ToString() == "1";
                SqlSelector selector = new SqlSelector(context, fDataSet);
                using (selector)
                {
                    string userIdName = context.GetSqlParamName("USER_ID");
                    string sql = string.Format(ObjectUtil.SysCulture, FUNC_SQL, userIdName,
                        context.ContextConfig.GetFunction("LENGTH", "FN_TREE_LAYER"));
                    selector.Select("SYS_FUNCTION", sql, builder.Parameters);

                    Dictionary<int, FunctionItem> idFunctions = new Dictionary<int, FunctionItem>();

                    DataTable table = fDataSet.Tables["SYS_FUNCTION"];
                    foreach (DataRow row in table.Rows)
                    {
                        FunctionItem item = new FunctionItem(row);
                        if (item.IsLeaf)
                        {
                            try
                            {
                                idFunctions.Add(item.Id, item);
                                fFunctions.Add(item.Key, item);
                            }
                            catch
                            {
                            }
                        }
                    }

                    sql = string.Format(ObjectUtil.SysCulture, SUB_FUNC_SQL, userIdName);
                    selector.Select("SYS_SUB_FUNC", sql, builder.Parameters);
                    table = fDataSet.Tables["SYS_SUB_FUNC"];
                    var group = from item in table.AsEnumerable()
                                group item by item.Field<int>("FnId");
                    foreach (var groupItem in group)
                    {
                        FunctionItem item;
                        if (idFunctions.TryGetValue(groupItem.Key, out item))
                            item.AddSubFunctions(groupItem);
                    }
                }
            }
        }

        public IEnumerable<string> GetSubFunctions(string key)
        {
            FunctionItem item;
            if (fFunctions.TryGetValue(key, out item))
                return item.SubFunctions;

            return Enumerable.Empty<string>();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (fDataSet != null)
                    fDataSet.Dispose();
            }
        }

        public IEnumerable<OperatorConfig> GetSubOperators(OperatorPage page, string functionKey)
        {
            if (string.IsNullOrEmpty(functionKey))
                return null;

            FunctionItem item = ObjectUtil.TryGetValue(fFunctions, functionKey);
            if (item == null)
                return null;

            return item.GetOperators(page);
        }

        //public static IEnumerable<OperatorConfig> SafeGetSubOperators(OperatorPage page, string functionKey)
        //{
        //    TkDebug.ThrowIfNoGlobalVariable();
        //    SimpleFunctionRight functionRight = WebGlobalVariable.SessionGbl.AppRight.FunctionRight
        //        as SimpleFunctionRight;
        //    if (functionRight == null)
        //        return null;

        //    return functionRight.GetSubOperators(page, functionKey);
        //}
    }
}