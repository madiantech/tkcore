using System;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseSingleDbDetailSource : BaseSingleDbMetaDataSource, IDetailEvent
    {
        private DataRow fMainRow;

        protected BaseSingleDbDetailSource()
        {
        }

        protected BaseSingleDbDetailSource(IEditDbConfig config)
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

        protected internal void SetMainRow(DataRow row)
        {
            fMainRow = row;
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
                FillUpdateTables(MainResolver, input);
                if (input.Style.Style != PageStyle.Delete && !input.IsPost)
                    MainResolver.AddVirtualFields();
            }
        }

        protected virtual void FillUpdateTables(TableResolver resolver, IInputData input)
        {
            fMainRow = resolver.Query(input.QueryString);
        }

        protected virtual void DefaultUpdateAction(IInputData input)
        {
            ProcessFillingUpdateEvent(input);
            FillUpdateTables(input);

            FilledUpdateEventArgs e = new FilledUpdateEventArgs(input);
            OnFilledUpdateTables(e);

            if (SupportData)
                CheckDataRight(input, MainRow);
        }
    }
}
