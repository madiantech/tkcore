using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class DbParameterList : IEnumerable<IDbParameter>
    {
        public DbParameterList()
        {
            List = new List<IDbParameter>();
        }

        #region IEnumerable<IDbParameter> 成员

        public IEnumerator<IDbParameter> GetEnumerator()
        {
            if (List == null)
                return Enumerable.Empty<IDbParameter>().GetEnumerator();
            else
                return List.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        [ObjectElement(IsMultiple = true, LocalName = "Parameter", ObjectType = typeof(DbParameter))]
        internal List<IDbParameter> List { get; private set; }

        internal int Count
        {
            get
            {
                return List == null ? 0 : List.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return List == null || List.Count == 0;
            }
        }

        public void Add(IDbParameter parameter)
        {
            TkDebug.AssertArgumentNull(parameter, "parameter", this);

            List.Add(parameter);
        }

        public void Add(string fieldName, TkDataType dataType, object fieldValue)
        {
            TkDebug.AssertArgumentNullOrEmpty(fieldName, "fieldName", this);

            List.Add(new DbParameter(fieldName, dataType, fieldValue));
        }

        public void Add(IFieldInfo fieldInfo, object fieldValue)
        {
            TkDebug.AssertArgumentNull(fieldInfo, "fieldInfo", this);

            Add(fieldInfo.FieldName, fieldInfo.DataType, fieldValue);
        }

        public void AddRange(IEnumerable<IDbParameter> parameters)
        {
            TkDebug.AssertArgumentNull(parameters, "parameters", this);

            List.AddRange(parameters);
        }

        public void Add(DbParameterList parameterList)
        {
            foreach (DbParameter parameter in parameterList.List)
                Add(parameter.FieldName, parameter.DataType, parameter.FieldValue);
        }
    }
}
