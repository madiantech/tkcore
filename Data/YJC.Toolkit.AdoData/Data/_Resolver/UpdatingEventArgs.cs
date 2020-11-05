using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class UpdatingEventArgs : EventArgs
    {
        internal UpdatingEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the UpdatingEventArgs class.
        /// </summary>
        public UpdatingEventArgs(DataRow row, UpdateKind status, UpdateKind invokeMethod,
            DataSet postDataSet, DataRow postDataRow, IInputData inputData)
        {
            SetProperties(row, status, invokeMethod, postDataRow, postDataSet, inputData);
        }

        public DataRow Row { get; private set; }

        public UpdateKind Status { get; private set; }

        public UpdateKind InvokeMethod { get; private set; }

        public DataSet PostDataSet { get; private set; }

        public DataRow PostDataRow { get; private set; }

        public IInputData InputData { get; private set; }

        internal void SetProperties(DataRow row, UpdateKind status, UpdateKind invokeMethod,
            DataRow postDataRow, DataSet postDataSet, IInputData inputData)
        {
            Row = row;
            Status = status;
            InvokeMethod = invokeMethod;
            PostDataRow = postDataRow;
            PostDataSet = postDataRow == null ? postDataSet : postDataRow.Table.DataSet;
            InputData = inputData;
        }
    }
}