using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseMultipleDbDetailSource : BaseMultipleDbMetaDataSource, IDetailEvent
    {
        private DataRow fMainRow;

        protected BaseMultipleDbDetailSource()
        {
        }

        protected BaseMultipleDbDetailSource(IEditDbConfig config)
            : base(config)
        {
        }

        #region IDetailEvent 成员

        public event EventHandler<FillingUpdateEventArgs> FillingUpdateTables
        {
            add
            {
                EventHandlers.AddHandler(EventConst.FillingUpdateEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.FillingUpdateEvent, value);
            }
        }

        public event EventHandler<FilledUpdateEventArgs> FilledUpdateTables
        {
            add
            {
                EventHandlers.AddHandler(EventConst.FilledUpdateEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.FilledUpdateEvent, value);
            }
        }

        #endregion

        protected FillingUpdateEventArgs FillingUpdateArgs { get; private set; }

        protected DataRow MainRow
        {
            get
            {
                if (fMainRow == null)
                {
                    var rows = MainResolver.HostTable.Rows;
                    TkDebug.Assert(rows.Count > 0, string.Format(ObjectUtil.SysCulture,
                        "至少应该从表{0}获得1条记录，但是当前为0", MainResolver.TableName), this);
                    fMainRow = rows[0];
                }
                return fMainRow;
            }
        }

        protected virtual void OnFillingUpdateTables(FillingUpdateEventArgs e)
        {
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.FillingUpdateEvent, this, e);
        }

        protected virtual void OnFilledUpdateTables(FilledUpdateEventArgs e)
        {
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.FilledUpdateEvent, this, e);
        }

        protected virtual void AddFillingUpdateTables(FillingUpdateEventArgs e)
        {
            FillingUpdateArgs.Handled.AddResolvers(MainResolver);
            foreach (var item in ChildTables)
                FillingUpdateArgs.Handled.AddResolvers(item.Resolver);
        }

        protected void ProcessFillingUpdateEvent(IInputData input)
        {
            FillingUpdateArgs = new FillingUpdateEventArgs(input);
            AddFillingUpdateTables(FillingUpdateArgs);
            OnFillingUpdateTables(FillingUpdateArgs);
        }

        protected virtual void FillUpdateTables(IInputData input)
        {
            if (!FillingUpdateArgs.Handled.IsHandled(MainResolver))
            {
                FillUpdateTables(MainResolver, input, null);
            }
            foreach (var childInfo in ChildTables)
            {
                if (!FillingUpdateArgs.Handled.IsHandled(childInfo.Resolver))
                    FillUpdateTables(childInfo.Resolver, input, childInfo);
            }
        }

        protected virtual void FillUpdateTables(TableResolver resolver, IInputData input,
            ChildTableInfo childInfo)
        {
            if (childInfo == null)
                fMainRow = resolver.Query(input.QueryString);
            else
                childInfo.FillDetailTables(MainResolver);
        }

        protected virtual void DefaultUpdateAction(IInputData input)
        {
            ProcessFillingUpdateEvent(input);
            FillUpdateTables(input);
            OnFilledUpdateTables(new FilledUpdateEventArgs(input));

            if (SupportData)
                CheckDataRight(input, MainRow);
        }
    }
}
