using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-10-27",
        Description = "根据查询条件，生成查询结果的数据源。这里只需要配置计算结果的数据源")]
    internal class QuerySourceConfig : IConfigCreator<ISource>
    {
        internal class MakeConditionSource : BaseDbSource, IConfigCreator<ISource>, ISupportMetaData
        {
            private IMetaData fMetaData;

            #region IConfigCreator<ISource> 成员

            public ISource CreateObject(params object[] args)
            {
                return this;
            }

            #endregion

            #region ISupportMetaData 成员

            public bool CanUseMetaData(IPageStyle style)
            {
                return true;
            }

            public void SetMetaData(IPageStyle style, IMetaData metaData)
            {
                fMetaData = metaData;
            }

            #endregion

            public override OutputData DoAction(IInputData input)
            {
                Tk5ListMetaData metaData = fMetaData as Tk5ListMetaData;
                if (metaData != null)
                {
                    string tableName = metaData.Table.TableName;
                    var scheme = fMetaData.GetTableScheme(tableName);
                    MetaDataTableResolver resolver = new MetaDataTableResolver(scheme, this);
                    using (resolver)
                    {
                        DataTable table = resolver.CreateVirtualTable();
                        table.TableName = "Condition";
                        DataRow row = table.NewRow();
                        resolver.SetDefaultValue(row);
                        table.Rows.Add(row);
                        resolver.FillCodeTable((PageStyleClass)PageStyle.List);
                    }
                }

                return OutputData.Create(DataSet);
            }
        }

        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            var source = new CompositeSource();
            source.Add(input => input.IsPost, CalcSource);
            source.Add(input => !input.IsPost, new MakeConditionSource());

            return source;
        }

        #endregion

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(SourceConfigFactory.REG_NAME)]
        public IConfigCreator<ISource> CalcSource { get; private set; }
    }
}
