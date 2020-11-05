using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2015-03-25", Description = "维护标准代码表的数据源")]
    internal class StdCodeTableSourceConfig : IConfigCreator<ISource>
    {
        private class PinyinSource : BaseDbSource
        {
            private readonly StdCodeTableResolver fResolver;

            public PinyinSource(StdCodeTableSourceConfig config, string tableName)
            {
                fResolver = config.CreateTableRsolver(this, tableName);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    fResolver.Dispose();

                base.Dispose(disposing);
            }

            public override OutputData DoAction(IInputData input)
            {
                fResolver.Select("CODE_PY IS NULL OR CODE_PY = ''");
                DataTable table = fResolver.HostTable;
                foreach (DataRow row in table.Rows)
                {
                    row["Py"] = PinYinUtil.GetPyHeader(row["Name"].ToString(), string.Empty);
                }
                fResolver.SetCommands(AdapterCommand.Update);
                fResolver.UpdateDatabase();

                return OutputData.CreateToolkitObject(KeyData.Empty);
            }
        }

        internal class ChangeDelSource : ChangeStatusSource
        {
            private readonly string fTableName;

            public ChangeDelSource(StdCodeTableSourceConfig config, string tableName, string status)
                : base((source) => config.CreateTableRsolver(source, tableName), "Del", status)
            {
                fTableName = tableName;
            }

            public override OutputData DoAction(IInputData input)
            {
                OutputData result = base.DoAction(input);
                RemoveCache(fTableName);

                return result;
            }
        }

        private class StdCodeTableListSource : DbListSource
        {
            private static string GetModuleCreator(StdCodeTableSourceConfig config)
            {
                return string.IsNullOrEmpty(config.ModuleCreator) ? string.Empty
                    : "/" + config.ModuleCreator;
            }

            public StdCodeTableListSource(StdCodeTableSourceConfig config, string tableName)
            {
                if (!string.IsNullOrEmpty(config.Context))
                    Context = DbContextUtil.CreateDbContext(config.Context);

                MainResolver = config.CreateTableRsolver(this, tableName);
                UseMetaData = false;
                PageSize = int.MaxValue;
                SortQuery = true;
                OrderBy = "ORDER BY " + DecoderConst.DEFAULT_ORDER;
                if (config.UseQueryString)
                {
                    var operators = new StandardOperatorsConfig
                    {
                        Right = new SimpleListOperateRightConfig { Operators = UpdateKind.All },
                        Operators = new List<IConfigCreator<OperatorConfig>>()
                    };
                    var url = new MarcoConfigItem(true, true,
                        string.Format("~/c/{{ModuleCreator}}/insert/{{CcSource}}?{0}={{#{0}}}", config.TableName));
                    operators.Operators.Add(new OperatorConfig(RightConst.INSERT, "新建", OperatorPosition.Global,
                        "Dialog", null, "icon-plus", url));
                    url = new MarcoConfigItem(true, true,
                        string.Format("~/c/{{ModuleCreator}}/update/{{CcSource}}?{0}={{#{0}}}", config.TableName));
                    operators.Operators.Add(new OperatorConfig(RightConst.UPDATE, "修改", OperatorPosition.Row,
                        "Dialog", null, "icon-edit", url)
                    { UseKey = true });
                    operators.Operators.Add(OperatorConfig.DeleteOperator);
                    Operators = operators;
                }
                else
                {
                    var pinyinOper = config.AutoPinyin ? DataString.AutoPinyinOperator : string.Empty;
                    string oper = string.Format(ObjectUtil.SysCulture, DataString.StdCodeOperators, pinyinOper);
                    var operators = oper.ReadXmlFromFactory
                        <IConfigCreator<IOperatorsConfig>>(OperatorsConfigFactory.REG_NAME);
                    Operators = operators.CreateObject();
                }
            }
        }

        private class StdCodeTableEditSource : SingleDbEditSource
        {
            private readonly string fTableName;

            public StdCodeTableEditSource(StdCodeTableSourceConfig config, string tableName)
            {
                if (!string.IsNullOrEmpty(config.Context))
                    Context = DbContextUtil.CreateDbContext(config.Context);

                MainResolver = config.CreateTableRsolver(this, tableName);
                UseMetaData = false;
                fTableName = tableName;
            }

            protected override void OnCommittedData(CommittedDataEventArgs e)
            {
                base.OnCommittedData(e);

                RemoveCache(fTableName);
            }
        }

        private class StdCodeTableInsertSource : SingleDbInsertSource
        {
            public StdCodeTableInsertSource(StdCodeTableSourceConfig config, string tableName)
            {
                if (!string.IsNullOrEmpty(config.Context))
                    Context = DbContextUtil.CreateDbContext(config.Context);

                MainResolver = config.CreateTableRsolver(this, tableName);
                UseMetaData = false;
            }
        }

        private class StdCodeTableDeleteSource : SingleDbDeleteSource
        {
            private readonly string fTableName;

            public StdCodeTableDeleteSource(StdCodeTableSourceConfig config, string tableName)
            {
                if (!string.IsNullOrEmpty(config.Context))
                    Context = DbContextUtil.CreateDbContext(config.Context);

                MainResolver = config.CreateTableRsolver(this, tableName);
                fTableName = tableName;
            }

            protected override void OnCommittedData(CommittedDataEventArgs e)
            {
                base.OnCommittedData(e);

                RemoveCache(fTableName);
            }
        }

        private class StdCodeTableDetailSource : SingleDbDetailSource
        {
            public StdCodeTableDetailSource(StdCodeTableSourceConfig config, string tableName)
            {
                if (!string.IsNullOrEmpty(config.Context))
                    Context = DbContextUtil.CreateDbContext(config.Context);

                MainResolver = config.CreateTableRsolver(this, tableName);
                UseMetaData = false;
            }
        }

        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);
            string tableName = UseQueryString ? input.QueryString[TableName] : TableName;

            var style = input.Style.Style;
            if (!input.IsPost)
            {
                switch (style)
                {
                    case PageStyle.Insert:
                        return new StdCodeTableInsertSource(this, tableName);

                    case PageStyle.Delete:
                        return new StdCodeTableDeleteSource(this, tableName);

                    case PageStyle.Update:
                    case PageStyle.Detail:
                        return new StdCodeTableDetailSource(this, tableName);

                    case PageStyle.List:
                        return new StdCodeTableListSource(this, tableName);

                    case PageStyle.Custom:
                        switch (input.Style.Operation)
                        {
                            case "Pinyin":
                                return new PinyinSource(this, tableName);

                            case "FakeDelete":
                                return new ChangeDelSource(this, tableName, "1");

                            case "Restore":
                                return new ChangeDelSource(this, tableName, "0");
                        }
                        break;
                }
            }
            else
            {
                switch (style)
                {
                    case PageStyle.Insert:
                    case PageStyle.Update:
                        return new StdCodeTableEditSource(this, tableName);

                    case PageStyle.List:
                        return new StdCodeTableListSource(this, tableName);
                }
            }

            return null;
        }

        #endregion IConfigCreator<ISource> 成员

        [SimpleAttribute]
        public string TableName { get; private set; }

        [SimpleAttribute]
        public string Context { get; private set; }

        [SimpleAttribute]
        public bool UseQueryString { get; private set; }

        [SimpleAttribute]
        public string ModuleCreator { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool AutoPinyin { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public SimpleIdCreator AutoKeyCreator { get; private set; }

        private StdCodeTableResolver CreateTableRsolver(IDbDataSource source, string tableName)
        {
            StdCodeTableResolver resolver = new StdCodeTableResolver(tableName, AutoKeyCreator, source);
            return resolver;
        }

        private static void RemoveCache(string regName)
        {
            var factory = BaseGlobalVariable.Current.FactoryManager.GetCodeFactory(
                CodeTablePlugInFactory.REG_NAME).Convert<CodeTablePlugInFactory>();
            factory.RemoveStdCodeTable(regName);
        }
    }
}