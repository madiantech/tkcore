using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ChangeStatusSource : BaseDbSource
    {
        public ChangeStatusSource(TableResolver resolver, string nickName, string status)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", null);
            TkDebug.AssertArgumentNull(status, "status", null);

            Resolver = resolver;
            NickName = nickName;
            Status = status;
        }

        public ChangeStatusSource(Func<BaseDbSource, TableResolver> createFunc, string nickName, string status)
        {
            TkDebug.AssertArgumentNull(createFunc, "createFunc", null);
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", null);
            TkDebug.AssertArgumentNull(status, "status", null);

            Resolver = createFunc(this);
            NickName = nickName;
            Status = status;
        }

        internal ChangeStatusSource(ChangeStatusSourceConfig config)
        {
            Resolver = config.Resolver.CreateObject(this);
            NickName = config.NickName;
            Status = Expression.Execute(config.Status, this);
            UpdateTrackField = config.UpdateTrackField;
        }

        public TableResolver Resolver { get; private set; }

        public string NickName { get; private set; }

        public string Status { get; private set; }

        public bool UpdateTrackField { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Resolver.DisposeObject();
            }

            base.Dispose(disposing);
        }

        public override OutputData DoAction(IInputData input)
        {
            try
            {
                DataRow row = Resolver.Query(input.QueryString);
                row.BeginEdit();
                try
                {
                    row[NickName] = Status;
                    if (UpdateTrackField)
                        Resolver.UpdateTrackField(UpdateKind.Update, row);
                }
                finally
                {
                    row.EndEdit();
                }
                Resolver.SetCommands(AdapterCommand.Update);
                Resolver.UpdateDatabase();

                return OutputData.CreateToolkitObject(Resolver.CreateKeyData());
            }
            catch (WebPostException ex)
            {
                return OutputData.CreateToolkitObject(ex.CreateErrorResult());
            }
        }
    }
}