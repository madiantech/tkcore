using System;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data.Constraint;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class MultipleDbEditSource : BaseMultipleDbEditSource, IEditEvent
    {
        public MultipleDbEditSource()
        {
        }

        public MultipleDbEditSource(IEditDbConfig config)
            : base(config)
        {
        }

        #region IEditEvent 成员

        public event EventHandler<FilledInsertEventArgs> FilledInsertTables
        {
            add
            {
                EventHandlers.AddHandler(EventConst.FilledInsertEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.FilledInsertEvent, value);
            }
        }

        public event EventHandler<PreparePostObjectEventArgs> PreparedPostObject
        {
            add
            {
                EventHandlers.AddHandler(EventConst.PreparedPostEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.PreparedPostEvent, value);
            }
        }

        #endregion IEditEvent 成员

        protected override bool TestPageStyleForMetaData(IPageStyle style)
        {
            return MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Insert) ||
                MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Update);
        }

        private void PostData(IInputData input)
        {
            DataSet postDataSet = input.PostObject.Convert<DataSet>();
            FieldErrorInfoCollection errors = new FieldErrorInfoCollection();
            var childResolvers = ChildResolvers.ToArray();
            MetaDataTableResolver metaResolver = MainResolver as MetaDataTableResolver;
            if (metaResolver != null)
                metaResolver.CheckFirstConstraints(input, errors);
            foreach (var item in childResolvers)
            {
                MetaDataTableResolver childResolver = item as MetaDataTableResolver;
                if (childResolver != null)
                    childResolver.CheckFirstConstraints(input, errors);
            }
            switch (input.Style.Style)
            {
                case PageStyle.Insert:
                    MainResolver.Insert(postDataSet, input);
                    foreach (var item in childResolvers)
                        item.Insert(postDataSet, input);
                    break;

                case PageStyle.Update:
                    MainResolver.Update(postDataSet, input);
                    foreach (var item in childResolvers)
                        item.Update(postDataSet, input);
                    break;
            }
            if (metaResolver != null)
                metaResolver.CheckLaterConstraints(input, errors);
            foreach (var item in childResolvers)
            {
                MetaDataTableResolver childResolver = item as MetaDataTableResolver;
                if (childResolver != null)
                    childResolver.CheckLaterConstraints(input, errors);
            }
            errors.CheckError();
        }

        private OutputData DoPost(IInputData input)
        {
            PreparePostObject(input);

            switch (input.Style.Style)
            {
                case PageStyle.Insert:
                    DefaultInsertAction(input);
                    break;

                case PageStyle.Update:
                    DefaultUpdateAction(input);
                    break;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    break;
            }

            PostData(input);
            Commit(input);

            return OutputData.CreateToolkitObject(MainResolver.CreateKeyData());
        }

        protected virtual void OnFilledInsertTables(FilledInsertEventArgs e)
        {
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.FilledInsertEvent, this, e);
        }

        protected virtual void OnPreparedPostObject(PreparePostObjectEventArgs e)
        {
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.PreparedPostEvent, this, e);
        }

        protected virtual void PreparePostObject(IInputData input)
        {
            DataSet postDataSet = input.PostObject.Convert<DataSet>();

            MainResolver.PrepareDataSet(postDataSet);
            foreach (var item in ChildTables)
            {
                item.Resolver.PrepareDataSet(postDataSet);
                item.SetRelationFieldValue(MainResolver);
            }

            OnPreparedPostObject(new PreparePostObjectEventArgs(input));
        }

        protected virtual void FillInsertTables()
        {
            MainResolver.SelectTableStructure();
            foreach (var item in ChildTables)
                item.Resolver.SelectTableStructure();
        }

        protected override void FillUpdateTables(TableResolver resolver, IInputData input, ChildTableInfo childInfo)
        {
            if (childInfo == null)
            {
                DataSet postDataSet = input.PostObject.Convert<DataSet>();
                resolver.Query(postDataSet);
            }
            else
                childInfo.FillDetailTables(MainResolver);
        }

        protected void DefaultInsertAction(IInputData input)
        {
            FillInsertTables();
            OnFilledInsertTables(new FilledInsertEventArgs(input));
        }

        public sealed override OutputData DoAction(IInputData input)
        {
            try
            {
                if (!input.IsPost)
                    throw new WebPostException("此Source只支持Post操作，当前是Get操作");

                Prepare();
                return DoPost(input);
            }
            catch (WebPostException ex)
            {
                return OutputData.CreateToolkitObject(ex.CreateErrorResult());
            }
        }
    }
}