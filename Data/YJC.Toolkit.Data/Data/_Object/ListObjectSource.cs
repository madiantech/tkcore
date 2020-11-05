using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ListObjectSource : BaseObjectSource<IListObjectSource>
    {
        public ListObjectSource(IListObjectSource source)
            : base(source)
        {
        }

        internal ListObjectSource(IListObjectSource source, ListObjectSourceConfig config)
            : base(source)
        {
            PageSize = config.PageSize;
            if (config.Operators != null)
                Operators = config.Operators.CreateObject();
        }

        public override OutputData DoAction(IInputData input)
        {
            if (input.Style.Style == PageStyle.List)
            {
                ObjectListModel model = DoListAction(input);
                input.CallerInfo.AddInfo(model.CallerInfo);
                return OutputData.CreateObject(model);
            }
            else
                return ErrorPageStyle(PageStyle.List, input);
        }

        public int PageSize { get; set; }

        public IObjectOperatorsConfig Operators { get; set; }

        private ObjectListModel FillListObject(IInputData input, int pageNumber,
            int pageSize, int start)
        {
            ObjectListModel model = new ObjectListModel();

            // 权限和数据过滤
            //ParamBuilderContainer condition = new ParamBuilderContainer();
            //condition.Add(MainResolver.CreateFakeDeleteBuilder());
            //condition.Add(CreateCustomCondition(input));
            //condition.Add(GetDataRight(input));
            //if (FilterSql != null)
            //{
            //    string sql = Expression.Execute(FilterSql, this);
            //    condition.Add(sql);
            //}

            // TabSheet的问题
            //if (TabSheets != null && TabSheets.Count > 0)
            //{
            //    var selectedTab = GetSelectTabSheet(input);
            //    selectedTab.Selected = true;
            //    condition.Add(selectedTab.ParamBuilder);
            //    DataSet.Tables.Add(TabSheets.CreateTable("TabSheet"));
            //}

            // 全局的操作权限，设置全局的按钮
            IObjectOperateRight operateRight = null;
            if (input.QueryString["GetData"] != "Page" && !input.IsPost)
            {
                //MainResolver.FillCodeTable(input.Style);
                CreateListOperators(model, input, ref operateRight);
            }

            // ListInfo和PageInfo
            var pageInfo = Source.CreatePageInfo(input, pageNumber, pageSize);
            model.SetPageInfo(pageInfo);

            if (model.Count.TotalCount <= 0)
                return model;

            // 分页选取数据
            int startNumber = pageSize * pageNumber + start;
            var items = Source.GetList(pageInfo.Item3, input, startNumber, pageSize);
            model.SetList(items, GetFields());

            // 每条Row的操作权限
            if (Operators != null)
            {
                if (operateRight == null)
                    operateRight = Operators.Right.CreateObject();
                var allOperators = Operators.Operators;
                if (allOperators != null)
                    CreateRowOperators(model, input, operateRight, allOperators);
            }

            return model;
            //int recCount = pageSize * pageNumber + start;
            //string whereSql = condition.IsEmpty ? string.Empty : "WHERE " + condition.Sql;
            //var listContext = FillListTable(MainResolver.ListFields, MainResolver.TableName,
            //    MainResolver.GetKeyFieldArray(), whereSql, orderby, recCount, pageSize);

            //SqlSelector selector = new SqlSelector(Context, DataSet);
            //using (selector)
            //{
            //    ISimpleAdapter adapter = selector;
            //    adapter.SetSql(listContext.ListSql, condition);
            //    Context.ContextConfig.SetListData(listContext, adapter, DataSet, recCount,
            //        pageSize, FillTableName);

            //    MainResolver.AddVirtualFields();

            // 每条Row的操作权限
            //    if (Operators != null)
            //    {
            //        if (operateRight == null)
            //            operateRight = Operators.Right.CreateObject();
            //        var allOperators = Operators.Operators;
            //        if (allOperators != null)
            //            CreateRowOperators(input, operateRight, allOperators);
            //    }
            // 代码表解码
            //    MainResolver.Decode(input.Style);


            //    OnFilledListTables(new FilledListEventArgs(input.IsPost, pageNumber, pageSize,
            //        pageInfo.TotalCount, orderby, MainResolver, input.PostObject));
            //}
        }

        private void CreateListOperators(ObjectListModel model, IInputData input,
            ref IObjectOperateRight operateRight)
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
                                       select new Operator(item, this, input);
                }
                else
                {
                    IEnumerable<string> rights = operateRight.GetOperator(
                        new ObjectOperateRightEventArgs(input.Style, null));
                    var allOpertors = Operators.Operators;
                    if (rights != null && allOpertors != null)
                        listOpertors = from item in allOpertors
                                       join right in rights on item.Id equals right
                                       where item.Position == OperatorPosition.Global
                                       select new Operator(item, this, input);
                }
                model.ListOperators = listOpertors;
            }
        }

        private void CreateRowOperators(ObjectListModel model, IInputData input,
            IObjectOperateRight operateRight, IEnumerable<OperatorConfig> allOperators)
        {
            var rowOperators = from item in allOperators
                               where item.Position == OperatorPosition.Row
                               select new Operator(item, this, input);
            model.RowOperators = rowOperators;

            var rowRights = from item in rowOperators
                            select item.Id;
            foreach (var item in model.List)
                item.SetOperateRight(input.Style, operateRight, rowRights);
        }

        protected ObjectListModel DoListAction(IInputData input)
        {
            int pageNumber = input.QueryString["Page"].Value<int>();
            int pageSize = input.QueryString["PageSize"].Value<int>(PageSize);
            if (pageSize == 0)
                pageSize = BaseGlobalVariable.Current.UserInfo.PageSize;
            int start = input.QueryString["Start"].Value<int>();

            return FillListObject(input, pageNumber, pageSize, start);
        }
    }
}
