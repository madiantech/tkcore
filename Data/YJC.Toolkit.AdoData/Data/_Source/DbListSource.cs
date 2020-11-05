using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DbListSource : BaseSingleDbMetaDataSource, IListEvent
    {
        public const string QUERY_TABLE_NAME = "_QueryData";
        private const string CONDITION_HEADER_NAME = "Condition"; //"~X-Condition";

        public const string TAB_STYLE_OPERATION = "TabCount";
        public static readonly IPageStyle TabStyle = PageStyleClass.FromString(TAB_STYLE_OPERATION);

        private List<IFieldInfo> fListFields;

        private string fFillTableName;

        protected DbListSource()
        {
        }

        public DbListSource(IListDbConfig config)
            : base(config)
        {
            PageSize = config.PageSize;
            OrderBy = config.OrderBy;
            SortQuery = config.SortQuery;
            FillTableName = config.FillTableName;
            if (config.TabSheets != null)
                TabSheets = config.TabSheets.CreateTabSheet(this, MainResolver);
            if (config.Operators != null)
                Operators = config.Operators.CreateObject();

            FilterSql = config.FilterSql;
        }

        #region IListEvent 成员

        public event EventHandler<FilledListEventArgs> FilledListTables
        {
            add
            {
                EventHandlers.AddHandler(EventConst.ListEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.ListEvent, value);
            }
        }

        #endregion IListEvent 成员

        public int PageSize { get; protected set; }

        public string OrderBy { get; protected set; }

        public MarcoConfigItem FilterSql { get; protected set; }

        public RegNameList<ListTabSheet> TabSheets { get; protected set; }

        public IOperatorsConfig Operators { get; protected set; }

        public bool SortQuery { get; protected set; }

        public string FillTableName
        {
            get
            {
                if (string.IsNullOrEmpty(fFillTableName))
                    return MainResolver.TableName;
                return fFillTableName;
            }
            set
            {
                fFillTableName = value;
            }
        }

        protected override bool TestPageStyleForMetaData(IPageStyle style)
        {
            return MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.List);
        }

        private ListTabSheet GetSelectTabSheet(IInputData input)
        {
            string tab = input.QueryString["Tab"];
            if (string.IsNullOrEmpty(tab))
                return TabSheets[0];
            ListTabSheet result = TabSheets[tab];
            return result ?? TabSheets[0];
        }

        protected virtual void CreateListOperators(IInputData input, ref IOperateRight operateRight)
        {
            if (Operators != null)
            {
                IEnumerable<Operator> listOpertors = null;
                operateRight = Operators.Right.CreateObject();
                if (operateRight == null)
                {
                    var allOpertors = Operators.Operators;
                    if (allOpertors != null)
                        listOpertors = from item in allOpertors
                                       where item.Position == OperatorPosition.Global
                                       select new Operator(item, this, input, MainResolver.GetKeyFieldArray());
                }
                else
                {
                    IEnumerable<string> rights = operateRight.GetOperator(
                        new OperateRightEventArgs(input.Style, input.SourceInfo.Source, null));
                    var allOpertors = Operators.Operators;
                    if (rights != null && allOpertors != null)
                        listOpertors = from item in allOpertors
                                       join right in rights on item.Id equals right
                                       where item.Position == OperatorPosition.Global
                                       select new Operator(item, this, input, MainResolver.GetKeyFieldArray());
                }
                if (listOpertors != null)
                {
                    DataTable table = listOpertors.CreateTable("ListOperator");
                    if (table != null)
                        DataSet.Tables.Add(table);
                }
            }
        }

        private void CreateRowOperators(IInputData input, IOperateRight operateRight,
            IEnumerable<OperatorConfig> allOperators)
        {
            var rowOperators = from item in allOperators
                               where item.Position == OperatorPosition.Row
                               select new Operator(item, this, input, MainResolver.GetKeyFieldArray());
            DataTable operTable = rowOperators.CreateTable("RowOperator");
            if (operTable == null)
                return;

            DataSet.Tables.Add(operTable);

            string rightStr = string.Empty;
            if (operateRight == null)
            {
                var rowRights = from item in rowOperators
                                select item.Id;
                rightStr = string.Format(ObjectUtil.SysCulture, "|{0}|", string.Join("|", rowRights));
            }
            DataTable table = DataSet.Tables[FillTableName];
            DataColumn operatorColumn = table.Columns.Add("_OPERATOR_RIGHT");
            foreach (DataRow row in table.Rows)
            {
                if (operateRight == null)
                {
                    row[operatorColumn] = rightStr;
                }
                else
                {
                    var args = new OperateRightEventArgs(input.Style, input.SourceInfo.Source, row);
                    IEnumerable<string> rights = operateRight.GetOperator(args);
                    if (rights != null)
                    {
                        rightStr = string.Join("|", rights);
                        if (!string.IsNullOrEmpty(rightStr))
                            row[operatorColumn] = "|" + rightStr + "|";
                    }
                }
            }
        }

        private CountInfo CreatePageInfo(IInputData input, int pageNumber,
            int pageSize, ParamBuilderContainer condition)
        {
            CountInfo pageInfo;
            int totalCount = input.QueryString["TotalCount"].Value<int>();
            if (totalCount <= 0 || input.IsPost)
            {
                int count = DbUtil.ExecuteScalar("SELECT COUNT(*) FROM " + GetTableName(Context),
                    Context, condition).Value<int>();
                pageInfo = new CountInfo(count, pageNumber, pageSize);
            }
            else
            {
                pageInfo = new CountInfo(totalCount,
                    input.QueryString["TotalPage"].Value<int>(), pageNumber, pageSize);
            }
            return pageInfo;
        }

        private FieldListOrder ParseSortParams2(IInputData input)
        {
            string jsonOrder = input.QueryString["JsonOrder"];
            FieldListOrder order;
            if (input.IsPost || string.IsNullOrEmpty(jsonOrder))
            {
                if (!string.IsNullOrEmpty(OrderBy))
                    order = FieldListOrder.FromSqlString(MainResolver, OrderBy);
                else
                    order = new FieldListOrder();
            }
            else
            {
                order = FieldListOrder.FromJson(jsonOrder);
            }

            return order;
        }

        //private void ParseSortParams(IInputData input, out string orderby, out int sort, out string order)
        //{
        //    sort = input.QueryString["Sort"].Value<int>(-1);
        //    order = null;
        //    if (sort == -1 || fListFields == null)
        //    {
        //        orderby = OrderBy;
        //        if (!string.IsNullOrEmpty(orderby) && fListFields != null)
        //        {
        //            int comma = orderby.IndexOf(',');
        //            string subOrder = comma >= 0 ? orderby.Substring(0, comma - 1) : orderby;
        //            sort = fListFields.FindIndex(field =>
        //                subOrder.IndexOf(field.FieldName, StringComparison.Ordinal) != -1);
        //            if (sort != -1 && subOrder.IndexOf("DESC", StringComparison.OrdinalIgnoreCase) != -1)
        //                order = "DESC";
        //        }
        //    }
        //    else
        //    {
        //        if (sort < 0 || sort >= fListFields.Count)
        //            sort = 0;
        //        order = input.QueryString["Order"].Value<string>(string.Empty);
        //        orderby = string.Format(ObjectUtil.SysCulture, "ORDER BY {0} {1}",
        //            fListFields[sort].FieldName, order);
        //    }
        //}

        private string SetGetListInfo(IInputData input, ParamBuilderContainer condition, ListSortInfo listInfo)
        {
            //string orderby;
            //int sort;
            //string order;
            var jsonOrder = ParseSortParams2(input);
            //ParseSortParams(input, out orderby, out sort, out order);
            string queryCon = input.QueryString[CONDITION_HEADER_NAME];
            if (!string.IsNullOrEmpty(queryCon))
            {
                queryCon = Uri.UnescapeDataString(queryCon);
                QueryCondition queryCondition = QueryCondition.FromEncodeString(queryCon);
                condition.Add(queryCondition.Builder);
                DataTable queryTable = queryCondition.QueryData.CreateTable("_QueryData");
                DataSet.Tables.Add(queryTable);

                MetaDataTableResolver metaResolver = MainResolver as MetaDataTableResolver;
                if (metaResolver != null)
                    metaResolver.DecodeQueryTable(queryTable);
            }
            PrepareConditionData(DataSet.Tables[QUERY_TABLE_NAME], input);

            //listInfo.SortField = sort;
            //listInfo.Order = order != "DESC" ? "ASC" : "DESC";
            listInfo.SqlCon = queryCon;
            string result = jsonOrder.ToSqlOrder(MainResolver);
            // 对于自定义排序，由于无法改变ASC和DESC，将在ToSqlOrder中重置，所以，这行代码要放在后面
            listInfo.JsonOrder = jsonOrder.ToJson();
            return result;
        }

        protected virtual void PrepareConditionData(DataTable table, IInputData input)
        {
        }

        private string SetPostListInfo(IInputData input, ParamBuilderContainer condition, ListSortInfo listInfo)
        {
            MetaDataTableResolver metaResolver = MainResolver as MetaDataTableResolver;
            if (metaResolver != null)
            {
                QueryConditionObject conditionData = input.PostObject.Convert<QueryConditionObject>();
                IParamBuilder builder = metaResolver.GetQueryCondition(conditionData);
                if (builder != null)
                {
                    QueryCondition queryCondition = new QueryCondition(conditionData.Condition, builder);
                    condition.Add(builder);
                    listInfo.SqlCon = queryCondition.ToEncodeString();
                }
            }
            if (SortQuery)
            {
                //string orderby;
                //int sort;
                //string order;
                var jsonOrder = ParseSortParams2(input);
                //ParseSortParams(input, out orderby, out sort, out order);
                //listInfo.SortField = sort;
                //listInfo.Order = order != "DESC" ? "ASC" : "DESC";
                listInfo.JsonOrder = jsonOrder.ToJson();

                return jsonOrder.ToSqlOrder(MainResolver);
            }

            return string.Empty;
        }

        protected virtual string GetTableName(TkDbContext context)
        {
            return MainResolver.GetSqlTableName(context);
        }

        protected override void OnSetMainResolver(TableResolver resolver)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", this);

            fListFields = resolver.ListFieldInfos;
        }

        private static bool IsListField(Tk5FieldInfoEx field, IPageStyle style)
        {
            var ctrl = field.Control.GetControl(style);
            if (ctrl == ControlType.Hidden)
                return false;
            if (field.ListDetail == null)
                return true;
            if (field.ListDetail.Search == FieldSearchMethod.Only)
                return false;
            return true;
        }

        protected override void OnReadMetaData(TableResolver resolver,
            IPageStyle style, ITableSchemeEx scheme)
        {
            fListFields = (from field in scheme.Fields
                           let tk5field = field.Convert<Tk5FieldInfoEx>()
                           where IsListField(tk5field, style)
                           orderby field.Control.GetOrder(style)
                           select MetaDataTableResolver.GetSortField(field)).ToList();
        }

        protected void DoListAction(IInputData input)
        {
            int pageNumber = input.QueryString["Page"].Value<int>();
            int pageSize = input.QueryString["PageSize"].Value<int>(PageSize);
            if (pageSize == 0)
                pageSize = BaseGlobalVariable.Current.UserInfo.PageSize;
            int start = input.QueryString["Start"].Value<int>();

            FillListDataSet(input, pageNumber, pageSize, start);
        }

        protected virtual IParamBuilder CreateCustomCondition(IInputData input)
        {
            return null;
        }

        protected virtual void OnFilledListTables(FilledListEventArgs e)
        {
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.ListEvent, this, e);
        }

        protected virtual IParamBuilder GetDataRight(IInputData input)
        {
            if (SupportData && DataRight != null)
            {
                ListDataRightEventArgs e = new ListDataRightEventArgs(Context,
                    BaseGlobalVariable.Current.UserInfo, MainResolver);
                return DataRight.GetListSql(e);
            }
            return null;
        }

        private ParamBuilderContainer CreateListCondition(IInputData input)
        {
            ParamBuilderContainer condition = new ParamBuilderContainer();
            condition.Add(MainResolver.CreateFixCondition());
            condition.Add(CreateCustomCondition(input));
            condition.Add(GetDataRight(input));
            if (FilterSql != null)
            {
                string sql = Expression.Execute(FilterSql, this);
                condition.Add(sql);
            }
            return condition;
        }

        protected void FillListDataSet(IInputData input, int pageNumber, int pageSize, int start)
        {
            ParamBuilderContainer condition = CreateListCondition(input);

            IOperateRight operateRight = null;
            if (TabSheets != null && TabSheets.Count > 0)
            {
                var selectedTab = GetSelectTabSheet(input);
                selectedTab.Selected = true;
                condition.Add(selectedTab.ParamBuilder);
                DataSet.Tables.Add(TabSheets.CreateTable("TabSheet"));
            }

            if (input.QueryString["GetData"] != "Page" && !input.IsPost)
            {
                MainResolver.FillCodeTable(input.Style);
                CreateListOperators(input, ref operateRight);
            }

            ListSortInfo listInfo = new ListSortInfo(input);
            string orderby = null;
            if (input.IsPost)
                orderby = SetPostListInfo(input, condition, listInfo);
            else
                orderby = SetGetListInfo(input, condition, listInfo);

            CountInfo pageInfo = CreatePageInfo(input, pageNumber, pageSize, condition);

            DataSet.Tables.Add(EnumUtil.Convert(pageInfo).CreateTable("Count"));
            DataSet.Tables.Add(EnumUtil.Convert(listInfo).CreateTable("Sort"));

            if (pageInfo.TotalCount <= 0)
            {
                OnFilledListTables(new FilledListEventArgs(input.IsPost, pageNumber, pageSize,
                    pageInfo.TotalCount, orderby, MainResolver, input.PostObject, condition));
                return;
            }

            int recCount = pageSize * pageNumber + start;
            string whereSql = condition.IsEmpty ? string.Empty : "WHERE " + condition.Sql;
            var listContext = FillListTable(MainResolver.ListFields, GetTableName(Context),
                MainResolver.GetKeyFieldArray(), whereSql, orderby, recCount, pageSize);

            SqlSelector selector = new SqlSelector(Context, DataSet);
            using (selector)
            {
                ISimpleAdapter adapter = selector;
                adapter.SetSql(listContext.ListSql, condition);
                Context.ContextConfig.SetListData(listContext, adapter, DataSet, recCount,
                    pageSize, FillTableName);

                MainResolver.AddVirtualFields();

                if (Operators != null)
                {
                    if (operateRight == null)
                        operateRight = Operators.Right.CreateObject();
                    var allOperators = Operators.Operators;
                    if (allOperators != null)
                        CreateRowOperators(input, operateRight, allOperators);
                }
                MainResolver.Decode(input.Style);

                OnFilledListTables(new FilledListEventArgs(input.IsPost, pageNumber, pageSize,
                    pageInfo.TotalCount, orderby, MainResolver, input.PostObject, condition));
            }
        }

        protected virtual IListSqlContext FillListTable(string selectFields, string tableName,
            IFieldInfo[] keyArray, string whereSql, string orderBy, int start, int pageSize)
        {
            return Context.ContextConfig.GetListSql(selectFields, tableName, keyArray,
                whereSql, orderBy, start, start + pageSize);
        }

        public override OutputData DoAction(IInputData input)
        {
            if (input.Style.Style == PageStyle.List)
            {
                DoListAction(input);
                input.CallerInfo.AddInfo(DataSet);
            }
            else if (MetaDataUtil.Equals(input.Style, TabStyle))
            {
                var result = DoTabCountAction(input);
                return OutputData.CreateToolkitObject(result);
            }
            else
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "当前支持页面类型为List，当前类型是{0}", input.Style), this);

            return OutputData.Create(DataSet);
        }

        protected virtual TabConditionCount DoTabCountAction(IInputData input)
        {
            TabConditionCount result = new TabConditionCount();
            if (TabSheets != null && TabSheets.Count > 0)
            {
                ParamBuilderContainer condition = CreateListCondition(input);
                string sql = string.Format(ObjectUtil.SysCulture,
                    "SELECT COUNT(*) FROM {0}", GetTableName(Context));
                string queryCon = input.QueryString[CONDITION_HEADER_NAME];
                if (!string.IsNullOrEmpty(queryCon))
                {
                    queryCon = Uri.UnescapeDataString(queryCon);
                    QueryCondition queryCondition = QueryCondition.FromEncodeString(queryCon);
                    condition.Add(queryCondition.Builder);
                }
                foreach (var tabItem in TabSheets)
                {
                    IParamBuilder builder = condition.IsEmpty ? tabItem.ParamBuilder :
                        ParamBuilder.CreateParamBuilder(condition, tabItem.ParamBuilder);
                    int count = DbUtil.ExecuteScalar(sql, Context, builder).Value<int>();
                    result.Add(tabItem.Id, count);
                }
            }

            return result;
        }
    }
}