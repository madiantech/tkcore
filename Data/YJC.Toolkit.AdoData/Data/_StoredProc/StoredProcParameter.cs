using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class StoredProcParameter
    {
        private bool fNeedFillValue;
        private object fValue;
        /// <summary>
        /// Initializes a new instance of the StoredProcParameter class.
        /// </summary>
        public StoredProcParameter(string name, ParameterDirection direction, TkDataType type)
        {
            Name = name;
            Direction = direction;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the StoredProcParameter class.
        /// </summary>
        public StoredProcParameter(string name, ParameterDirection direction, TkDataType type, int size)
        {
            Name = name;
            Direction = direction;
            Type = type;
            Size = size;
        }

        public string Name { get; private set; }

        public ParameterDirection Direction { get; private set; }

        public TkDataType Type { get; private set; }

        public int Size { get; set; }

        public object Value
        {
            get
            {
                return fValue;
            }
            set
            {
                if (fValue != value)
                {
                    fValue = value;
                    if (DbUtil.IsInputParameter(Direction))
                    {
                        if (Parameter != null)
                            Parameter.Value = value;
                        else
                            fNeedFillValue = true;
                    }

                }
            }
        }

        public IDbDataParameter Parameter { get; private set; }

        internal void SetDefaultValue(object value)
        {
            if (!fNeedFillValue)
            {
                Value = value;
            }
        }

        public void FillDataParameter(IDbDataParameter parameter)
        {
            Parameter = parameter;
            parameter.ParameterName = Name;
            parameter.Direction = Direction;
            if (Size > 0)
                parameter.Size = Size;
            if (DbUtil.IsInputParameter(Direction))
            {
                parameter.Value = Value;
                fNeedFillValue = false;
            }
        }

        public void SetInputValue()
        {
            if (fNeedFillValue)
            {
                Parameter.Value = fValue;
                fNeedFillValue = false;
            }
        }

        public void SetOutputValue()
        {
            if (DbUtil.IsOutputParameter(Direction))
                Value = Parameter.Value;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "存储过程参数{0}", Name);
        }
    }
}
