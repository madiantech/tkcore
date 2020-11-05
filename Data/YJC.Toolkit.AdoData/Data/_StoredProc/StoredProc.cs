using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class StoredProc : AbstractStoredProc
    {
        private readonly Dictionary<string, StoredProcParameter> fInputParams
            = new Dictionary<string, StoredProcParameter>();
        private readonly Dictionary<string, StoredProcParameter> fOutputParams
            = new Dictionary<string, StoredProcParameter>();
        private readonly List<StoredProcParameter> fParams = new List<StoredProcParameter>();
        private bool fParamCreated;
        private bool fValueChanged;

        public StoredProc(string procName)
            : base(procName)
        {
        }

        public StoredProc(string procName, IDbConnection connection)
            : base(procName, connection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the StoredProc class.
        /// </summary>
        public StoredProc(string procName, TkDbContext context)
            : base(procName, context)
        {
        }

        internal List<StoredProcParameter> Parameters
        {
            get
            {
                return fParams;
            }
        }

        public object this[string paramName]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(paramName, "paramName", this);

                if (fOutputParams.ContainsKey(paramName))
                    return fOutputParams[paramName].Value;
                else
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "输出参数中没有名称为{0}的参数", paramName), this);
                return null;
            }
            set
            {
                TkDebug.AssertArgumentNullOrEmpty(paramName, "paramName", this);

                fValueChanged = true;
                if (fInputParams.ContainsKey(paramName))
                    fInputParams[paramName].Value = value;
                else
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "输入参数中没有名称为{0}的参数", paramName), this);
            }
        }

        public T GetParamValue<T>(string paramName) where T : class
        {
            return this[paramName] as T;
        }

        public void AddParameter(string name, ParameterDirection direction, TkDataType type,
            int size, object value)
        {
            if (fParamCreated)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "存储过程{0}已经执行过了，不能再添加参数了", ProcName), this);
                return;
            }

            StoredProcParameter param = Add(name, direction, type);
            if (size > 0)
                param.Size = size;
            if (value != null)
                param.Value = value;
            fParams.Add(param);
            if (DbUtil.IsInputParameter(direction))
                fInputParams.Add(name, param);
            if (direction != ParameterDirection.Input)
                fOutputParams.Add(name, param);
        }

        public void AddParameter(string name, ParameterDirection direction, TkDataType type, int size)
        {
            AddParameter(name, direction, type, size, null);
        }

        public void AddParameter(string name, ParameterDirection direction, TkDataType type)
        {
            AddParameter(name, direction, type, 0);
        }

        public void AddParameter(string name, ParameterDirection direction)
        {
            AddParameter(name, direction, TkDataType.Int);
        }

        public void AddParameter(string name)
        {
            AddParameter(name, ParameterDirection.Input);
        }

        protected override void PrepareParameters()
        {
            foreach (StoredProcParameter item in fParams)
                Command.Parameters.Add(CreateDataParameter(item));
            fParamCreated = true;
        }

        protected sealed override void SetInputValues()
        {
            if (fValueChanged)
            {
                foreach (StoredProcParameter param in fInputParams.Values)
                    param.SetInputValue();
                fValueChanged = false;
            }
        }

        protected sealed override void SetOutputValues()
        {
            foreach (StoredProcParameter param in fOutputParams.Values)
                param.SetOutputValue();
        }
    }
}
