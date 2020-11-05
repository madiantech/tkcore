using System.Collections;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    internal sealed class InternalConstraintList : IEnumerable<BaseConstraint>
    {
        private readonly List<BaseConstraint> fList;

        public InternalConstraintList()
        {
            fList = new List<BaseConstraint>();
        }

        #region IEnumerable<BaseConstraint> 成员

        IEnumerator<BaseConstraint> IEnumerable<BaseConstraint>.GetEnumerator()
        {
            return fList.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return fList.GetEnumerator();
        }

        #endregion

        internal void RemoveConstraint<T>() where T : BaseConstraint
        {
            for (int i = fList.Count - 1; i >= 0; --i)
            {
                var constraint = fList[i];
                if (constraint is T)
                    fList.RemoveAt(i);
            }
        }

        public void Add(BaseConstraint constraint)
        {
            fList.Add(constraint);
        }

        public void CheckPostDataSet(TkDbContext context, IInputData inputData,
            string tableName, FieldErrorInfoCollection errorObjects)
        {
            DataSet ds = inputData.PostObject.Convert<DataSet>();
            DataTable table = ds.Tables[tableName];
            if (table == null)
                return;

            foreach (BaseConstraint constraint in fList)
            {
                string fieldName = constraint.Field.NickName;
                if (!table.Columns.Contains(fieldName))
                {
                    if (constraint.CoerceCheck)
                        constraint.InternalCheckError(inputData, null, -1, context);
                    continue;
                }

                int i = 0;
                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                    {
                        ++i;
                        continue;
                    }
                    string value = row[fieldName].ToString();
                    if (!errorObjects.Contains(constraint, i))
                    {
                        FieldErrorInfo error = constraint.InternalCheckError(inputData, value, i,
                            context, row, table, ds);
                        if (error != null)
                            errorObjects.Add(constraint, error, i);
                    }
                    ++i;
                }
            }
        }

        public void Clear()
        {
            fList.Clear();
        }
    }
}
