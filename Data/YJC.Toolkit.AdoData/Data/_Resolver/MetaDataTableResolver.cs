using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data.Constraint;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class MetaDataTableResolver : TableResolver
    {
        private IEnumerable<IFieldInfoEx> fVirtualFields;
        private readonly ITableSchemeEx fSourceSchemeEx;
        private string fListFields;
        private readonly CodeTableList fCodeTables;
        private bool fInitCodeTable;
        private readonly EasySearchList fEasySearches;
        private bool fInitEasySearch;
        private ITableSchemeEx fCurrentSchemeEx;
        private readonly INonUIResolvers fNonUIResolvers;
        private ListSearchCollection fListSearches;
        private Constraint.ConstraintCollection fConstraints;
        private bool fHasAddConstraint;
        private List<IFieldInfo> fListFieldInfos;
        private bool fHasUpload;
        private bool fInitializedUpload;
        private List<(IFieldUpload Upload, IUploadProcessor2 Processor)> fUploadProcessors;

        public MetaDataTableResolver(ITableSchemeEx scheme, IDbDataSource source)
            : base(MetaDataUtil.ConvertToTableScheme(scheme), source)
        {
            fCurrentSchemeEx = fSourceSchemeEx = scheme;
            fVirtualFields = from item in scheme.Fields
                             where item.Kind != FieldKind.Data
                             select item;
            CalcSourceListFields();
            fCodeTables = new CodeTableList();
            fEasySearches = new EasySearchList();
            fInitCodeTable = false;
            fNonUIResolvers = source as INonUIResolvers;
            ListSource = source as IListEvent;
            DetailSource = source as IDetailEvent;
            //DetailListSource = fSource as IDetailListEvent;
            //UpdateSource = fSource as IUpdateEvent;
            EditSource = source as IEditEvent;
            CommitSource = source as ICommitEvent;
            if (CommitSource != null)
                CommitSource.CommittedData += CommitSource_CommittedData;
        }

        protected CodeTableList CodeTables
        {
            get
            {
                return fCodeTables;
            }
        }

        public override IFieldInfo this[string nickName] => fCurrentSchemeEx[nickName];

        protected ITableSchemeEx SourceSchemeEx
        {
            get
            {
                return fSourceSchemeEx;
            }
        }

        protected ITableSchemeEx CurrentSchemeEx
        {
            get
            {
                return fCurrentSchemeEx;
            }
        }

        protected Constraint.ConstraintCollection Constraints
        {
            get
            {
                if (fConstraints == null)
                    fConstraints = new Constraint.ConstraintCollection(TableName, HostDataSet);
                return fConstraints;
            }
        }

        public override List<IFieldInfo> ListFieldInfos
        {
            get
            {
                return fListFieldInfos;
            }
        }

        public override string ListFields
        {
            get
            {
                return fListFields;
            }
        }

        public INonUIResolvers NonUIResolvers
        {
            get
            {
                TkDebug.AssertNotNull(fNonUIResolvers, "传入的Source不支持INonUIResolvers接口，请确认", this);
                return fNonUIResolvers;
            }
        }

        public IListEvent ListSource { get; private set; }

        public IDetailEvent DetailSource { get; private set; }

        //public IUpdateEvent UpdateSource { get; private set; }

        public IEditEvent EditSource { get; private set; }

        public ICommitEvent CommitSource { get; private set; }

        public ListSearchCollection ListSearches
        {
            get
            {
                if (fListSearches == null)
                    fListSearches = new ListSearchCollection();
                return fListSearches;
            }
        }

        //private static bool IsListField(IFieldInfoEx field)
        //{
        //    return field.Control != null && (field.Control.DefaultShow & PageStyle.List) == PageStyle.List
        //        && field.Kind == FieldKind.Data;
        //}

        private static string GetSelectField(TkDbContext context, IFieldInfo field)
        {
            if (field.FieldName == field.NickName)
                return context.EscapeName(field.NickName);
            return string.Format(ObjectUtil.SysCulture, "{0} {1}",
                context.EscapeName(field.FieldName), context.EscapeName(field.NickName));
        }

        //private static bool IsShowInList(PageStyleClass listStyle, IFieldInfoEx field)
        //{
        //    ControlType ctrlType = field.Control.GetControl(listStyle);
        //    return ctrlType != ControlType.Hidden && ctrlType != ControlType.None;
        //}

        private void CommitSource_CommittedData(object sender, CommittedDataEventArgs e)
        {
            if (fHasUpload)
            {
                foreach (var item in fUploadProcessors)
                    item.Processor.AfterSave();
            }
        }

        private void CalcSourceListFields()
        {
            PageStyleClass listStyle = (PageStyleClass)PageStyle.List;
            //fListFieldInfos = (from field in fSourceSchemeEx.Fields
            //                   where IsListField(field) && IsShowInList(listStyle, field)
            //                   orderby field.Control.GetOrder(listStyle)
            //                   select GetSortField(field)).ToList();
            fListFieldInfos = (from field in fSourceSchemeEx.Fields
                               where field.IsShowInList(listStyle, true)
                               orderby field.Control.GetOrder(listStyle)
                               select GetSortField(field)).ToList();
            //var fields = from field in fSourceSchemeEx.Fields
            //             where IsListField(field)
            //             select GetSelectField(Context, field);
            var fields = from field in fSourceSchemeEx.Fields
                         where field.IsShowInList(listStyle, false)
                         select GetSelectField(Context, field);
            var uploadFields = CalcUploadFields();
            fields = fields.Union(uploadFields).Distinct();
            fListFields = string.Join(", ", fields);
        }

        private IEnumerable<string> CalcUploadFields()
        {
            //var uploadFields = from field in fSourceSchemeEx.Fields
            //                   where IsListField(field) && field.Upload != null
            //                   select field;
            var uploadFields = from field in fSourceSchemeEx.Fields
                               where field.IsShowInList((PageStyleClass)PageStyle.List, true) && field.Upload != null
                               select field;

            foreach (IFieldInfoEx field in uploadFields)
            {
                var upload = field.AssertUpload();
                IUploadProcessor2 processor = upload.CreateUploadProcessor2();
                IEnumerable<string> selectFields = processor.GetListSelectFields(Context,
                    upload, fSourceSchemeEx);
                foreach (var item in selectFields)
                    yield return item;
            }
        }

        private void AddConstraints(IPageStyle style)
        {
            if (!fHasAddConstraint)
            {
                Constraints.Clear();
                SetConstraints(style);
                //foreach (EasySearchField field in fEasySearches)
                //    Constraints.Add(field.CreateConstraint());
                fHasAddConstraint = true;
            }
        }

        private static bool IsDecoderType(IFieldInfoEx item, DecoderType decoderType)
        {
            if (item.Decoder == null)
                return false;
            if (item.Decoder.Type == DecoderType.None)
                return false;
            return (item.Decoder.Type & decoderType) == item.Decoder.Type;
        }

        private IFieldInfoEx[] GetCodeTables(DecoderType decoderType)
        {
            ITableSchemeEx schemeEx = fCurrentSchemeEx;
            if (schemeEx == null)
                return null;

            var codeTables = from item in schemeEx.Fields
                             where IsDecoderType(item, decoderType)
                             select item;
            var result = codeTables.ToArray();
            if (result.Length == 0)
                return null;

            return result;
        }

        public override void AddVirtualFields()
        {
            var columns = HostTable.Columns;
            foreach (var item in fVirtualFields)
            {
                Type type = MetaDataUtil.ConvertDataTypeToType(item.DataType);
                switch (item.Kind)
                {
                    case FieldKind.Calc:
                        columns.Add(item.NickName, type, item.Expression);
                        break;

                    case FieldKind.Virtual:
                        columns.Add(item.NickName, type);
                        break;
                }
            }
        }

        protected virtual IUploadProcessor2 CreateUploadProcessor(IFieldInfoEx info, UpdateKind status)
        {
            return null;
        }

        private IDecoder GetDecoder(IFieldInfoEx field, ControlType ctrlType)
        {
            string regName = field.Decoder.RegName;
            switch (field.Decoder.Type)
            {
                case DecoderType.CodeTable:
                    return fCodeTables.GetCodeTable(regName, ctrlType);

                case DecoderType.EasySearch:
                    return fEasySearches.GetEasySearch(regName, ctrlType);
            }
            return null;
        }

        protected virtual void SetConstraints(IPageStyle style)
        {
            foreach (IFieldInfoEx field in fCurrentSchemeEx.Fields)
            {
                if (field.Control == null)
                    continue;
                ControlType ctrl = field.Control.GetControl(style);
                switch (field.DataType)
                {
                    case TkDataType.String:
                        if (field.Length > 0 && ctrl.IsEmptyCtrl())
                            Constraints.Add(new StringLengthConstraint(field, field.Length));
                        break;

                    case TkDataType.Byte:
                    case TkDataType.Short:
                    case TkDataType.Long:
                    case TkDataType.Int:
                        if (ctrl.IsTextCtrl())
                            Constraints.Add(new IntConstraint(field));
                        break;

                    case TkDataType.Date:
                    case TkDataType.DateTime:
                        if (ctrl.IsDateCtrl())
                        {
                            Constraints.Add(new DateConstraint(field));
                            Constraints.Add(new DateRangeConstraint(field));
                        }
                        break;

                    case TkDataType.Double:
                    case TkDataType.Money:
                    case TkDataType.Decimal:
                        if (ctrl.IsTextCtrl())
                            Constraints.Add(new DoubleConstraint(field));
                        break;
                }

                if (!field.IsEmpty && ctrl.IsEmptyCtrl())
                    Constraints.Add(new NotEmptyConstraint(field));

                ITk5FieldInfo tk5Field = field as ITk5FieldInfo;
                if (tk5Field == null)
                    continue;

                if (tk5Field.Decoder != null && tk5Field.Decoder.Type == DecoderType.EasySearch)
                {
                    if (ctrl == ControlType.EasySearch)
                    {
                        var constraint = new EasySearchConstraint(tk5Field, tk5Field.Decoder.RegName);
                        Constraints.Add(constraint);
                    }
                }
                if (tk5Field.Constraints != null)
                {
                    foreach (var item in tk5Field.Constraints)
                    {
                        var constraint = item.CreateObject(field, this);
                        Constraints.Add(constraint);
                    }
                }
            }
        }

        protected override void OnMetaDataChanged(bool useSource, ITableScheme scheme)
        {
            if (useSource)
                CalcSourceListFields();
            else
                fListFields = Fields;
        }

        protected virtual void AddCodeTable(IPageStyle style, CodeTableList codeTables)
        {
            var fields = GetCodeTables(DecoderType.CodeTable);
            if (fields == null)
                return;

            BasePlugInFactory factory = BaseGlobalVariable.Current.FactoryManager.GetCodeFactory(
                CodeTablePlugInFactory.REG_NAME);
            foreach (var item in fields)
            {
                string regName = item.Decoder.RegName;
                if (!codeTables.ContainsKey(regName))
                {
                    CodeTable ct = factory.CreateInstance<CodeTable>(regName);
                    //ControlType ctrlType = item.Control.GetControl(style);
                    codeTables.Add(regName, ct);
                }
            }
        }

        protected virtual void AddEasySearch(IPageStyle style, EasySearchList easySearches)
        {
            var fields = GetCodeTables(DecoderType.EasySearch);
            if (fields == null)
                return;

            BasePlugInFactory factory = BaseGlobalVariable.Current.FactoryManager.GetCodeFactory(
                EasySearchPlugInFactory.REG_NAME);
            foreach (var item in fields)
            {
                string regName = item.Decoder.RegName;
                if (!easySearches.ContainsKey(regName))
                {
                    EasySearch ct = factory.CreateInstance<EasySearch>(regName);
                    easySearches.Add(regName, ct);
                }
            }
        }

        private void ExecuteUpdating(UpdatingEventArgs e, ITk5FieldInfo field)
        {
            e.Row[field.NickName] = Expression.Execute(field.Edit.Updating, this);
        }

        protected override void OnUpdatingRow(UpdatingEventArgs e)
        {
            base.OnUpdatingRow(e);

            ProcessUpload(e);

            var updatingFields = from field in CurrentSchemeEx.Fields
                                 where HasUpdatingItem(field)
                                 select field as ITk5FieldInfo;
            foreach (var field in updatingFields)
            {
                switch (e.Status)
                {
                    case UpdateKind.Insert:
                        if ((field.Edit.Updating.UpdateKind & UpdatingUpdateKind.Insert) == UpdatingUpdateKind.Insert)
                            ExecuteUpdating(e, field);
                        break;

                    case UpdateKind.Update:
                        if ((field.Edit.Updating.UpdateKind & UpdatingUpdateKind.Update) == UpdatingUpdateKind.Update)
                            ExecuteUpdating(e, field);
                        break;
                }
            }
        }

        private void ProcessUpload(UpdatingEventArgs e)
        {
            if (e.Status == UpdateKind.Delete && MoveDataFlag)
                return;

            if (!fInitializedUpload)
            {
                fInitializedUpload = true;
                var uploads = (from item in fCurrentSchemeEx.Fields
                               where item.Upload != null
                               select item).ToList();
                if (uploads.Count > 0)
                {
                    fHasUpload = true;
                    fUploadProcessors = new List<(IFieldUpload Upload, IUploadProcessor2 Processor)>();
                    foreach (IFieldInfoEx item in uploads)
                    {
                        IUploadProcessor2 processor = CreateUploadProcessor(item, e.Status);
                        if (processor != null)
                            fUploadProcessors.Add((item.Upload, processor));
                    }
                }
                else
                    fHasUpload = false;
            }
            if (fHasUpload)
            {
                foreach (var item in fUploadProcessors)
                {
                    DataRowFieldValueAccessor accessor = new DataRowFieldValueAccessor(e.Row, HostDataSet);
                    object result = item.Processor.Process(this, item.Upload, accessor, e.Status);
                    if (result != null && fNonUIResolvers != null)
                    {
                        TableResolver resolver = result as TableResolver;
                        if (resolver != null)
                        {
                            if (!fNonUIResolvers.NonUIResolvers.Add(resolver))
                                resolver.Dispose();
                        }
                    }
                }
            }
        }

        protected void InitializeCodeTable(IPageStyle style)
        {
            if (!fInitCodeTable)
            {
                fCodeTables.Clear();
                AddCodeTable(style, fCodeTables);
                fInitCodeTable = true;
            }
        }

        protected void InitializeEasySearch(IPageStyle style)
        {
            if (!fInitEasySearch)
            {
                fEasySearches.Clear();
                AddEasySearch(style, fEasySearches);
                fInitEasySearch = true;
            }
        }

        public override void ReadMetaData(ITableSchemeEx metaData)
        {
            base.ReadMetaData(metaData);

            if (metaData == null)
                fCurrentSchemeEx = fSourceSchemeEx;
            else
                fCurrentSchemeEx = metaData;

            fVirtualFields = from item in fCurrentSchemeEx.Fields
                             where item.Kind != FieldKind.Data
                             select item;
        }

        public virtual void FillCodeTable(IPageStyle style)
        {
            TkDebug.AssertArgumentNull(style, "style", this);

            InitializeCodeTable(style);
            var codeTables = fCodeTables.CreateEnumerable();
            foreach (CodeTable code in codeTables)
                code.Fill(HostDataSet, Context);
        }

        public virtual void FillCachedCodeTable(IPageStyle style)
        {
            TkDebug.AssertArgumentNull(style, "style", this);

            InitializeCodeTable(style);
            var codeTables = fCodeTables.CreateCachedEnumerable();
            foreach (CodeTable code in codeTables)
                code.Fill(HostDataSet, Context);
        }

        private void ImportCheckFirstConstraints(IInputData inputData, FieldErrorInfoCollection errorObjects)
        {
            TkDebug.AssertArgumentNull(inputData, "inputData", this);
            TkDebug.AssertArgumentNull(errorObjects, "errorObjects", this);

            //AddEasySearches(inputData.Style);
            AddConstraints(inputData.Style);
            Constraints.RemoveConstraints<EasySearchConstraint>();

            Constraints.CheckDbFirst(Context, inputData, errorObjects);
        }

        public void CheckFirstConstraints(IInputData inputData, FieldErrorInfoCollection errorObjects)
        {
            TkDebug.AssertArgumentNull(inputData, "inputData", this);
            TkDebug.AssertArgumentNull(errorObjects, "errorObjects", this);

            //AddEasySearches(inputData.Style);
            AddConstraints(inputData.Style);

            Constraints.CheckDbFirst(Context, inputData, errorObjects);
        }

        public void CheckLaterConstraints(IInputData inputData, FieldErrorInfoCollection errorObjects)
        {
            TkDebug.AssertArgumentNull(inputData, "inputData", this);
            TkDebug.AssertArgumentNull(errorObjects, "errorObjects", this);

            AddConstraints(inputData.Style);

            Constraints.CheckDbLater(Context, inputData, errorObjects);
        }

        public void DecodeQueryTable(DataTable queryTable)
        {
            if (queryTable == null || queryTable.Rows.Count == 0)
                return;

            IPageStyle style = (PageStyleClass)PageStyle.List;
            InitializeEasySearch(style);
            var fields = GetCodeTables(DecoderType.EasySearch);
            if (fields == null)
                return;

            TkDbContext context = Context;
            foreach (IFieldInfoEx field in fields)
            {
                if (!queryTable.Columns.Contains(field.NickName))
                    continue;

                string fieldName = field.NickName + "_Name";
                if (!queryTable.Columns.Contains(fieldName))
                    queryTable.Columns.Add(fieldName);

                ControlType ctrlType = field.Control != null ? field.Control.GetControl(style) : ControlType.Label;
                IDecoder decoder = GetDecoder(field, ctrlType);
                if (decoder != null)
                {
                    foreach (DataRow row in queryTable.Rows)
                    {
                        string value = row[field.NickName].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            IDecoderItem result = decoder.Decode(value, context);
                            if (result != null)
                                row[fieldName] = result.Name;
                        }
                    }
                }
            }
        }

        public void Decode(IPageStyle style)
        {
            TkDebug.AssertArgumentNull(style, "style", this);

            DataTable table = HostTable;
            if (table.Rows.Count == 0)
                return;

            InitializeCodeTable(style);
            InitializeEasySearch(style);
            var fields = GetCodeTables(DecoderType.CodeTable | DecoderType.EasySearch);
            if (fields == null)
                return;

            TkDbContext context = Context;
            foreach (IFieldInfoEx field in fields)
            {
                if (!table.Columns.Contains(field.NickName))
                    continue;

                string fieldName = field.NickName + "_Name";
                if (!table.Columns.Contains(fieldName))
                    table.Columns.Add(fieldName);

                ControlType ctrlType = field.Control != null ? field.Control.GetControl(style) : ControlType.Label;
                IDecoder decoder = GetDecoder(field, ctrlType);
                if (decoder != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        string value = row[field.NickName].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            IDecoderItem result = decoder.Decode(value, context);
                            if (result != null)
                            {
                                row.BeginEdit();
                                try
                                {
                                    row[fieldName] = result.Name;
                                    var additions = field.Decoder.Additions;
                                    if (additions != null)
                                    {
                                        foreach (var addition in additions)
                                        {
                                            string otherValue = result[addition.DecoderNickName];
                                            if (!string.IsNullOrEmpty(otherValue))
                                                try
                                                {
                                                    row[addition.DataNickName] = otherValue;
                                                }
                                                catch
                                                {
                                                }
                                        }
                                    }
                                }
                                finally
                                {
                                    row.EndEdit();
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static bool HasDefaultValue(IFieldInfoEx field)
        {
            ITk5FieldInfo tk5Field = field as ITk5FieldInfo;
            if (tk5Field == null)
                return false;
            return tk5Field.Edit != null && tk5Field.Edit.DefaultValue != null;
        }

        public override void SetDefaultValue(DataRow row)
        {
            base.SetDefaultValue(row);

            var defaultFields = from field in CurrentSchemeEx.Fields
                                where HasDefaultValue(field)
                                select field as ITk5FieldInfo;
            foreach (var field in defaultFields)
            {
                try
                {
                    row[field.NickName] = Expression.Execute(field.Edit.DefaultValue, this);
                }
                catch
                {
                }
            }
        }

        protected virtual void SetListSearches()
        {
            foreach (IFieldInfoEx field in CurrentSchemeEx.Fields)
            {
                ITk5FieldInfo tk5Field = field as ITk5FieldInfo;
                if (tk5Field == null)
                    continue;

                switch (tk5Field.DataType)
                {
                    case TkDataType.Date:
                        if (!ListSearches.Contains(tk5Field.NickName))
                        {
                            if (tk5Field.ListDetail != null && tk5Field.ListDetail.Span)
                                DefaultDateSpanSearch.Add(ListSearches, tk5Field);
                            else
                                ListSearches.Add(tk5Field, new DefaultDateSearch());
                        }
                        break;

                    case TkDataType.Int:
                    case TkDataType.Short:
                    case TkDataType.Byte:
                    case TkDataType.Long:
                    case TkDataType.Double:
                    case TkDataType.Decimal:
                    case TkDataType.Money:
                    case TkDataType.String:
                    case TkDataType.DateTime:
                        if (!ListSearches.Contains(tk5Field.NickName))
                        {
                            if (tk5Field.ListDetail != null && tk5Field.ListDetail.Span)
                                DefaultSpanSearch.Add(ListSearches, tk5Field);
                            else if (tk5Field.DataType != TkDataType.String)
                                ListSearches.Add(tk5Field, new DefaultEqualSearch());
                        }
                        break;
                }

                var ctrl = tk5Field.Control.GetControl((PageStyleClass)PageStyle.List);
                if (ctrl == ControlType.MultipleEasySearch || ctrl == ControlType.CheckBoxList)
                    if (!ListSearches.Contains(tk5Field.NickName))
                    {
                        ListSearches.Add(tk5Field, new QuoteStringListSearch());
                    }
            }
        }

        public IParamBuilder GetQueryCondition(QueryConditionObject conditionData)
        {
            TkDebug.AssertArgumentNull(conditionData, "conditionData", this);

            if (conditionData.Condition.Count == 0)
                return null;

            ParamBuilderContainer result = new ParamBuilderContainer();

            ListSearches.Clear();
            AddListSearches();
            SetListSearches();

            BaseListSearch defaultSearch = new DefaultListSearch();

            foreach (KeyValuePair<string, string> item in conditionData.Condition)
            {
                if (string.IsNullOrEmpty(item.Value))
                    continue;
                IFieldInfo field = null;
                BaseListSearch colSearch = ListSearches[item.Key];
                if (colSearch != null)
                    field = colSearch.FieldName;
                if (field == null)
                    field = this[item.Key];
                if (field == null)
                    continue;

                if (item.Value == "~")
                    result.Add(string.Format(ObjectUtil.SysCulture,
                        "({0} IS NULL OR {0} = '')", field.FieldName));
                else
                {
                    if (colSearch == null)
                        colSearch = defaultSearch;
                    colSearch.Context = Context;
                    colSearch.ConditionData = conditionData.Condition;
                    colSearch.IsEqual = conditionData.IsEqual;
                    IFieldInfo searchField = colSearch.FieldName ?? field;
                    result.Add(colSearch.GetCondition(searchField, item.Value));
                }
            }

            if (result.IsEmpty)
                return null;
            return result;
        }

        private void AddListSearches()
        {
            var listSearches = from field in CurrentSchemeEx.Fields
                               let tk5Field = field as ITk5FieldInfo
                               where tk5Field != null && tk5Field.ListDetail != null
                               && tk5Field.ListDetail.ListSearch != null
                               select tk5Field;
            foreach (var field in listSearches)
            {
                BaseListSearch listSearch = field.ListDetail.ListSearch.CreateObject(field, this);
                if (listSearch != null)
                    ListSearches.Add(field, listSearch);
            }
        }

        public FieldErrorInfoCollection Import(DataSet postDataSet, IInputData input)
        {
            TkDebug.AssertArgumentNull(postDataSet, "postDataSet", this);

            IInputData inputData = new InputDataProxy(input, (PageStyleClass)PageStyle.List, postDataSet);
            FieldErrorInfoCollection errors = new FieldErrorInfoCollection();
            ImportCheckFirstConstraints(inputData, errors);
            Insert(postDataSet, input);
            CheckLaterConstraints(inputData, errors);

            return errors;
        }

        private static bool HasUpdatingItem(IFieldInfoEx field)
        {
            ITk5FieldInfo tk5Field = field as ITk5FieldInfo;
            if (tk5Field == null)
                return false;
            return tk5Field.Edit != null && tk5Field.Edit.Updating != null;
        }

        internal static IFieldInfo GetSortField(IFieldInfoEx field)
        {
            Tk5FieldInfoEx tk5Field = field as Tk5FieldInfoEx;
            if (tk5Field != null)
            {
                if (tk5Field.ListDetail != null && !string.IsNullOrEmpty(tk5Field.ListDetail.SortField))
                    return new FieldItem(tk5Field.ListDetail.SortField);
            }
            return field;
        }
    }
}