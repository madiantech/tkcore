using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class ConstraintCollection
    {
        private readonly InternalConstraintList fFirstConstraints;
        private readonly InternalConstraintList fConstraints;

        public ConstraintCollection(string tableName, DataSet hostDataSet)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertArgumentNull(hostDataSet, "hostDataSet", null);

            TableName = tableName;
            HostDataSet = hostDataSet;
            fFirstConstraints = new InternalConstraintList();
            fConstraints = new InternalConstraintList();
        }

        public string TableName { get; private set; }

        public DataSet HostDataSet { get; private set; }

        public void Add(BaseConstraint constraint)
        {
            constraint.TableName = TableName;
            constraint.HostDataSet = HostDataSet;
            if (constraint.IsFirstCheck)
                fFirstConstraints.Add(constraint);
            else
                fConstraints.Add(constraint);
        }

        public void CheckDbFirst(TkDbContext context, IInputData inputData,
            FieldErrorInfoCollection errorObjects)
        {
            fFirstConstraints.CheckPostDataSet(context, inputData, TableName, errorObjects);
        }

        public void CheckDbLater(TkDbContext context, IInputData inputData,
            FieldErrorInfoCollection errorObjects)
        {
            fConstraints.CheckPostDataSet(context, inputData, TableName, errorObjects);
        }

        internal void RemoveConstraints<T>() where T : BaseConstraint
        {
            fFirstConstraints.RemoveConstraint<T>();
            fConstraints.RemoveConstraint<T>();
        }

        public void Clear()
        {
            fFirstConstraints.Clear();
            fConstraints.Clear();
        }
    }
}
