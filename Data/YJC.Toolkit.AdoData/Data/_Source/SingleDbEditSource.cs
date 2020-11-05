using System;
using System.Data;
using YJC.Toolkit.Data.Constraint;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SingleDbEditSource : BaseSingleDbEditSource, IEditEvent
    {
        protected SingleDbEditSource()
        {
        }

        public SingleDbEditSource(IEditDbConfig config)
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

        private void PostData(IInputData input)
        {
            DataSet postDataSet = input.PostObject.Convert<DataSet>();
            FieldErrorInfoCollection errors = new FieldErrorInfoCollection();
            MetaDataTableResolver metaResolver = MainResolver as MetaDataTableResolver;
            if (metaResolver != null)
                metaResolver.CheckFirstConstraints(input, errors);
            switch (input.Style.Style)
            {
                case PageStyle.Insert:
                    MainResolver.Insert(postDataSet, input);
                    break;

                case PageStyle.Update:
                    MainResolver.Update(postDataSet, input);
                    break;
            }
            if (metaResolver != null)
                metaResolver.CheckLaterConstraints(input, errors);

            errors.CheckError();
        }

        protected virtual OutputData DoPost(IInputData input)
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

        protected override bool TestPageStyleForMetaData(IPageStyle style)
        {
            return MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Insert) ||
                MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Update);
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
            OnPreparedPostObject(new PreparePostObjectEventArgs(input));
        }

        protected virtual void FillInsertTables()
        {
            MainResolver.SelectTableStructure();
        }

        protected override void FillUpdateTables(TableResolver resolver, IInputData input)
        {
            DataSet postDataSet = input.PostObject.Convert<DataSet>();
            resolver.Query(postDataSet);
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