using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public abstract class BaseConstraint
    {
        private int fPosition;

        protected BaseConstraint(IFieldInfo field)
        {
            Field = field;
        }

        public IFieldInfo Field { get; private set; }

        public string TableName { get; internal set; }

        public DataSet HostDataSet { get; internal set; }

        public bool IsFirstCheck { get; set; }

        internal bool CoerceCheck { get; set; }

        protected FieldErrorInfo CreateErrorObject(string msg)
        {
            TkDebug.AssertArgumentNullOrEmpty(msg, "msg", this);

            return new FieldErrorInfo(TableName, Field.NickName, msg, fPosition);
        }

        protected abstract FieldErrorInfo CheckError(IInputData inputData, string value,
            int position, params object[] args);

        internal FieldErrorInfo InternalCheckError(IInputData inputData, string value,
            int position, params object[] args)
        {
            fPosition = position;
            return CheckError(inputData, value, position, args);
        }
    }
}
