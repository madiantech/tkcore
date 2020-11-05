using System.Data;
using YJC.Toolkit.Data.Constraint;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DbDetailTableMaintainSource : BaseSingleDbEditSource
    {
        private string fParentKey;

        protected DbDetailTableMaintainSource()
        {
            UpdateMode = UpdateMode.Merge;
        }

        internal DbDetailTableMaintainSource(DbDetailTableMaintainSourceConfig config)
            : base(config)
        {
            UpdateMode = config.UpdateMode;
            MainResolver.UpdateMode = UpdateMode;
            ParentKey = config.ParentKey;
            QueryStringName = config.QueryStringName;
        }

        public UpdateMode UpdateMode { get; set; }

        public string ParentKey { get; set; }

        public string QueryStringName { get; set; }

        private void PostData(IInputData input)
        {
            DataSet postDataSet = input.PostObject.Convert<DataSet>();
            FieldErrorInfoCollection errors = new FieldErrorInfoCollection();
            MetaDataTableResolver metaResolver = MainResolver as MetaDataTableResolver;
            if (metaResolver != null)
                metaResolver.CheckFirstConstraints(input, errors);
            switch (input.Style.Style)
            {
                case PageStyle.Update:
                    MainResolver.Update(postDataSet, input);
                    break;
            }
            if (metaResolver != null)
                metaResolver.CheckLaterConstraints(input, errors);

            errors.CheckError();
        }

        private string GetParentKeyValue(IInputData input)
        {
            string queryName = string.IsNullOrEmpty(QueryStringName) ? ParentKey : QueryStringName;
            return input.QueryString[queryName];
        }

        protected override void OnSetMainResolver(TableResolver resolver)
        {
            base.OnSetMainResolver(resolver);

            if (resolver != null)
            {
                resolver.UpdateMode = UpdateMode;
                resolver.UpdatingRow += ResolverUpdatingRow;
            }
        }

        private void ResolverUpdatingRow(object sender, UpdatingEventArgs e)
        {
            if (e.Status == UpdateKind.Insert && e.InvokeMethod == UpdateKind.Update)
            {
                e.Row[ParentKey] = fParentKey;
            }
        }

        protected override bool TestPageStyleForMetaData(IPageStyle style)
        {
            return MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Update);
        }

        protected virtual OutputData DoPost(IInputData input)
        {
            MainResolver.PrepareDataSet(input.PostObject.Convert<DataSet>());

            switch (input.Style.Style)
            {
                case PageStyle.Update:
                    DefaultUpdateAction(input);
                    break;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    break;
            }

            PostData(input);
            Commit(input);

            return OutputData.CreateToolkitObject(KeyData.Empty);
        }

        protected override void FillUpdateTables(TableResolver resolver, IInputData input)
        {
            if (SupportData)
            {
                var fieldInfo = resolver.GetFieldInfo(ParentKey);
                ListDataRightEventArgs e = new ListDataRightEventArgs(Context,
                    BaseGlobalVariable.Current.UserInfo, resolver);
                var builder = DataRight.GetListSql(e);
                builder = ParamBuilder.CreateParamBuilder(
                    SqlParamBuilder.CreateEqualSql(Context, fieldInfo, fParentKey), builder);
                resolver.Select(builder);
            }
            else
                resolver.SelectWithParam(ParentKey, fParentKey);
        }

        protected override void DefaultUpdateAction(IInputData input)
        {
            ProcessFillingUpdateEvent(input);
            FillUpdateTables(input);

            FilledUpdateEventArgs e = new FilledUpdateEventArgs(input);
            OnFilledUpdateTables(e);

            if (SupportData)
            {
                foreach (DataRow row in MainResolver.HostTable.Rows)
                    CheckDataRight(input, row);
            }

            if (!input.IsPost)
            {
                MetaDataTableResolver metaResolver = MainResolver as MetaDataTableResolver;
                if (metaResolver != null)
                {
                    metaResolver.Decode(input.Style);

                    if (input.Style.Style == PageStyle.Update)
                        metaResolver.FillCodeTable(input.Style);
                }
            }
        }

        public sealed override OutputData DoAction(IInputData input)
        {
            try
            {
                fParentKey = GetParentKeyValue(input);
                if (input.IsPost)
                    return DoPost(input);
                else
                {
                    DefaultUpdateAction(input);
                    input.CallerInfo.AddInfo(DataSet);

                    return OutputData.Create(DataSet);
                }
            }
            catch (WebPostException ex)
            {
                return OutputData.CreateToolkitObject(ex.CreateErrorResult());
            }
        }
    }
}