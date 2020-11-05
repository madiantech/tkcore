using System;
using System.Data;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class TableRelation
    {
        private TableResolver fMasterResolver;
        private TableResolver fDetailResolver;

        //private const string NEW_KEY = "_NEW_KEY";
        private readonly string[] fMasterFields;
        private readonly string[] fDetailFields;
        private readonly RelationType fType;
        private readonly ManyToManyRelation fManyToMany;

        public TableRelation(string masterField, string detailField, MarcoConfigItem filterSql,
            string order, ManyToManyRelation manyToMany)
            : this(masterField, detailField, RelationType.OnlyFill, filterSql, order)
        {
            TkDebug.AssertArgumentNull(manyToMany, "manyToMany", null);

            fManyToMany = manyToMany;
        }

        public TableRelation(string[] masterFields, string[] detailFields,
            RelationType type, MarcoConfigItem filterSql, string order)
        {
            TkDebug.AssertEnumerableArgumentNullOrEmpty(masterFields, "masterFields", null);
            TkDebug.AssertEnumerableArgumentNullOrEmpty(detailFields, "detailFields", null);

            fMasterFields = masterFields;
            fDetailFields = detailFields;
            fType = type;
            FilterSql = filterSql;
            OrderBy = order;
        }

        public TableRelation(string masterField, string detailField,
            RelationType type, MarcoConfigItem filterSql, string order)
            : this(new string[] { masterField }, new string[] { detailField }, type, filterSql, order)
        {
        }

        public TableRelation(TableRelationConfig config)
            : this(config.MasterFields, config.DetailFields, config.Type, config.FilterSql, config.OrderBy)
        {
            Top = config.Top;
            if (config.ManyToMany != null)
            {
                fType = RelationType.OnlyFill;
                fManyToMany = config.ManyToMany;
            }
        }

        public MarcoConfigItem FilterSql { get; private set; }

        public string OrderBy { get; private set; }

        public int Top { get; set; }

        private IParamBuilder GetManyToManyParamBuilder(TableResolver detailResolver, object masterValue)
        {
            return ParamBuilder.CreateSql(string.Format(ObjectUtil.SysCulture,
                "{0} IN ({1})", detailResolver.GetFieldInfo(fDetailFields[0]).FieldName,
                fManyToMany.GetSubSelectSql(masterValue)));
        }

        private IParamBuilder GetDetailParamBuilder(TableResolver detailResolver, DataRow row, string filterSql)
        {
            ParamBuilderContainer container = new ParamBuilderContainer();
            container.Add(GetManyToManyParamBuilder(detailResolver,
                row[fMasterFields[0]]));
            container.Add(filterSql);
            return container;
        }

        private IParamBuilder ThrowErrorManyToManyMode()
        {
            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                "多对多模式下，关联字段只能有1个，当前为{0}个", fDetailFields.Length), this);
            return null;
        }

        public void SetSimpleFieldValue(TableResolver masterResolver, TableResolver detailResolver)
        {
            TkDebug.AssertArgumentNull(masterResolver, "masterResolver", this);
            TkDebug.AssertArgumentNull(detailResolver, "detailResolver", this);

            fMasterResolver = masterResolver;
            fDetailResolver = detailResolver;
            if ((fType & RelationType.MasterValue) == RelationType.MasterValue)
                fDetailResolver.UpdatingRow += SetSimpleDetailFieldValue;
            else if ((fType & RelationType.DetailValue) == RelationType.DetailValue)
                fMasterResolver.UpdatingRow += SetSimpleMasterFieldValue;
        }

        private void SetSimpleDetailFieldValue(object sender, UpdatingEventArgs e)
        {
            switch (e.Status)
            {
                case UpdateKind.Insert:
                case UpdateKind.Update:
                    DataRow masterRow = fMasterResolver.HostTable.Rows[0];
                    if (fDetailFields.Length == 1)
                        e.Row[fDetailFields[0]] = masterRow[fMasterFields[0]];
                    else
                    {
                        for (int i = 0; i < fMasterFields.Length; ++i)
                            e.Row[fDetailFields[i]] = masterRow[fMasterFields[i]];
                    }
                    break;
            }
        }

        private void SetSimpleMasterFieldValue(object sender, UpdatingEventArgs e)
        {
            switch (e.Status)
            {
                case UpdateKind.Insert:
                case UpdateKind.Update:
                    DataRow detailRow = fDetailResolver.HostTable.Rows[0];
                    if (fDetailFields.Length == 1)
                        e.Row[fMasterFields[0]] = detailRow[fDetailFields[0]];
                    else
                    {
                        for (int i = 0; i < fMasterFields.Length; ++i)
                            e.Row[fMasterFields[i]] = detailRow[fDetailFields[i]];
                    }
                    break;
            }
        }

        public IParamBuilder CreateDetailParamBuilder(TableResolver masterResolver,
            TableResolver detailResolver, IInputData input)
        {
            TkDebug.AssertNotNull(masterResolver, "masterResolver", this);
            TkDebug.AssertNotNull(detailResolver, "detailResolver", this);
            TkDebug.AssertNotNull(input, "input", this);

            string[] keyArray = Array.ConvertAll(masterResolver.GetKeyFieldArray(),
                field => field.NickName);
            if (ObjectUtil.ArrayEqual(keyArray, fMasterFields))
            {
                if (fDetailFields.Length == 1)
                {
                    if (fManyToMany == null)
                    {
                        return SqlParamBuilder.CreateEqualSql(detailResolver.Context,
                            detailResolver.GetFieldInfo(fDetailFields[0]),
                            input.QueryString[fMasterFields[0]]);
                    }
                    else
                    {
                        return GetManyToManyParamBuilder(detailResolver,
                            input.QueryString[fMasterFields[0]]);
                    }
                }
                else
                {
                    if (fManyToMany == null)
                    {
                        IParamBuilder[] builders = new IParamBuilder[fDetailFields.Length];
                        for (int i = 0; i < fDetailFields.Length; ++i)
                            builders[i] = SqlParamBuilder.CreateEqualSql(detailResolver.Context,
                                detailResolver.GetFieldInfo(fDetailFields[i]),
                                input.QueryString[fMasterFields[i]]);

                        return ParamBuilder.CreateParamBuilder(builders);
                    }
                    else
                    {
                        return ThrowErrorManyToManyMode();
                    }
                }
            }
            else
            {
                DataRow row = masterResolver.Query(input.QueryString);
                if (fDetailFields.Length == 1)
                {
                    if (fManyToMany == null)
                    {
                        return SqlParamBuilder.CreateEqualSql(detailResolver.Context,
                            detailResolver.GetFieldInfo(fDetailFields[0]), row[fMasterFields[0]]);
                    }
                    else
                    {
                        return GetManyToManyParamBuilder(detailResolver, row[fMasterFields[0]]);
                    }
                }
                else
                {
                    if (fManyToMany == null)
                    {
                        IParamBuilder[] builders = new IParamBuilder[fDetailFields.Length];
                        for (int i = 0; i < fDetailFields.Length; ++i)
                            builders[i] = SqlParamBuilder.CreateEqualSql(detailResolver.Context,
                                detailResolver.GetFieldInfo(fDetailFields[i]),
                                row[fMasterFields[i]]);

                        return ParamBuilder.CreateParamBuilder(builders);
                    }
                    else
                    {
                        return ThrowErrorManyToManyMode();
                    }
                }
            }
        }

        public void FillDetailTable(TableResolver masterResolver, TableResolver detailResolver)
        {
            TkDebug.AssertNotNull(masterResolver, "masterResolver", this);
            TkDebug.AssertNotNull(detailResolver, "detailResolver", this);

            if ((fType & RelationType.OnlyFill) != RelationType.OnlyFill)
                return;
            DataTable table = masterResolver.HostTable;
            TkDebug.AssertNotNull(table, string.Format(ObjectUtil.SysCulture, "主表{0}不存在",
                masterResolver.TableName), this);

            foreach (DataRow row in table.Rows)
            {
                string filterSql = FilterSql == null ? string.Empty
                    : Expression.Execute(FilterSql, masterResolver.Source);
                if (Top == 0)
                {
                    if (fDetailFields.Length == 1)
                    {
                        if (fManyToMany == null)
                        {
                            detailResolver.SelectWithParam(filterSql, OrderBy,
                                fDetailFields[0], row[fMasterFields[0]]);
                        }
                        else
                        {
                            IParamBuilder container = GetDetailParamBuilder(detailResolver, row, filterSql);

                            detailResolver.Select(container, OrderBy);
                        }
                    }
                    else
                    {
                        if (fManyToMany == null)
                        {
                            object[] masterValues = new object[fMasterFields.Length];
                            for (int i = 0; i < fMasterFields.Length; ++i)
                                masterValues[i] = row[fMasterFields[i]];
                            detailResolver.SelectWithParams(filterSql, OrderBy, fDetailFields, masterValues);
                        }
                        else
                        {
                            ThrowErrorManyToManyMode();
                        }
                    }
                }
                else
                {
                    if (fManyToMany == null)
                    {
                        object[] values = (from item in fMasterFields select row[item]).ToArray();
                        IParamBuilder builder = detailResolver.CreateParamBuilder(filterSql, fDetailFields, values);
                        detailResolver.SelectTopRows(Top, builder, OrderBy);
                    }
                    else
                    {
                        if (fDetailFields.Length == 1)
                        {
                            IParamBuilder container = GetDetailParamBuilder(detailResolver, row, filterSql);

                            detailResolver.SelectTopRows(Top, container, OrderBy);
                        }
                        else
                            ThrowErrorManyToManyMode();
                    }
                }
            }
            detailResolver.AddVirtualFields();
        }

        //public override string ToString()
        //{
        //    try
        //    {
        //        return string.Format(ObjectUtil.SysCulture, "表[{0}]与表[{1}]的TableRelation对象",
        //            MasterResolver.TableName, DetailResolver.TableName);
        //    }
        //    catch
        //    {
        //        return base.ToString();
        //    }
        //}
    }
}
