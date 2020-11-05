using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class MultipleDbInsertSource : BaseDbSource, ISupportMetaData
    {
        private readonly List<Tuple<TableResolver, bool>> fDetailResolvers;

        protected MultipleDbInsertSource()
        {
            fDetailResolvers = new List<Tuple<TableResolver, bool>>();
        }

        public MultipleDbInsertSource(IBaseDbConfig config)
            : this()
        {
            TkDebug.AssertArgumentNull(config, "config", null);

            SetConfig(config);
            IMultipleResolverConfig multiple = config as IMultipleResolverConfig;
            if (multiple != null)
            {
                SetMainResolver(multiple.MainResolver.CreateObject(this));
                if (multiple.ChildResolvers != null)
                {
                    foreach (var item in multiple.ChildResolvers)
                        AddDetailTableResolver(item.Resolver.CreateObject(this), item.IsNewEmptyRow);
                }
            }
            if (config.DataRight != null)
            {
                SupportData = config.SupportData;
                DataRight = config.DataRight.CreateObject(MainResolver);
            }
            IEditDbConfig totalConfig = config as IEditDbConfig;
            if (totalConfig != null)
                UseMetaData = totalConfig.UseMetaData;
        }

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            if (style.Style == PageStyle.Insert)
                return UseMetaData;
            return false;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            MainResolver.ReadMetaData(metaData.GetTableScheme(MainResolver.TableName));
            foreach (var resolver in fDetailResolvers)
                resolver.Item1.ReadMetaData(metaData.GetTableScheme(resolver.Item1.TableName));
        }

        #endregion ISupportMetaData 成员

        public bool SupportData { get; protected set; }

        public bool UseMetaData { get; set; }

        public TableResolver MainResolver { get; protected set; }

        public IDataRight DataRight { get; protected set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                MainResolver.Dispose();
                foreach (var item in fDetailResolvers)
                    item.Item1.Dispose();
            }

            base.Dispose(disposing);
        }

        protected void DoInsertAction(IInputData input)
        {
            DataTable table = MainResolver.CreateVirtualTable();
            DataRow row = NewTableRow(table);
            MainResolver.SetDefaultValue(input.QueryString);
            MainResolver.SetDefaultValue(row);
            DecodeResolver(input, MainResolver);

            foreach (var item in fDetailResolvers)
            {
                TableResolver resolver = item.Item1;
                table = resolver.CreateVirtualTable();
                if (!item.Item2)
                {
                    row = NewTableRow(table);
                    resolver.SetDefaultValue(row);
                }
                DecodeResolver(input, resolver);
            }
        }

        public void SetMainResolver(TableResolver resolver)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", this);

            TkDebug.Assert(MainResolver == null, string.Format(ObjectUtil.SysCulture,
                "MainResolver已经设置，当前MainResolver的表名是{0}",
                MainResolver == null ? string.Empty : MainResolver.TableName), this);

            MainResolver = resolver;
        }

        public void AddDetailTableResolver(TableResolver resolver)
        {
            AddDetailTableResolver(resolver, false);
        }

        public void AddDetailTableResolver(TableResolver resolver, bool isNewEmptyRow)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", this);

            fDetailResolvers.Add(Tuple.Create(resolver, isNewEmptyRow));
        }

        public override OutputData DoAction(IInputData input)
        {
            PageStyle style = input.Style.Style;
            if (style == PageStyle.Insert)
            {
                DoInsertAction(input);
                input.CallerInfo.AddInfo(DataSet);
            }
            else
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "当前支持页面类型为Insert，当前类型是{0}", input.Style), this);

            return OutputData.Create(DataSet);
        }

        private static void DecodeResolver(IInputData input, TableResolver resolver)
        {
            MetaDataTableResolver metaResolver = resolver as MetaDataTableResolver;
            if (metaResolver != null)
            {
                metaResolver.FillCodeTable(input.Style);
                metaResolver.Decode(input.Style);
            }
        }

        private static DataRow NewTableRow(DataTable table)
        {
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return row;
        }
    }
}