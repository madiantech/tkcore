using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class CreateWorkflowOperateRight : IOperateRight
    {
        #region IOperateRight 成员

        public IEnumerable<string> GetOperator(OperateRightEventArgs e)
        {
            DataRow row = e.Row;
            string applyField, endField;
            if (string.IsNullOrEmpty(WfPrefix))
            {
                applyField = "IsApplyWf";
                endField = "WfIsEnd";
            }
            else
            {
                applyField = WfPrefix + "IsApplyWf";
                endField = WfPrefix + "WfIsEnd";
            }
            if (row != null)
            {
                var columns = row.Table?.Columns;
                if (columns != null && columns.Contains(applyField) && columns.Contains(endField))
                {
                    int applyValue = row[applyField].Value<int>();
                    FinishType finishType = row[endField].Value<FinishType>();
                    switch (finishType)
                    {
                        case FinishType.None:
                            if (applyValue == 1)
                                return Enumerable.Empty<string>();
                            else
                                return ReadOnly ? new string[] { WfCreateOperatorConfig.ID } :
                                    new string[] { RightConst.UPDATE, RightConst.DELETE, WfCreateOperatorConfig.ID };

                        case FinishType.Normal:
                            return Enumerable.Empty<string>();

                        case FinishType.ModifiedNormal:
                            return ReadOnly ? new string[] { WfCreateOperatorConfig.ID } :
                                new string[] { RightConst.UPDATE, WfCreateOperatorConfig.ID };

                        case FinishType.ReUse:
                            return new string[] { WfCreateOperatorConfig.ID };
                    }
                }

                return Enumerable.Empty<string>();
            }
            return ReadOnly ? Enumerable.Empty<string>() : new string[] { RightConst.INSERT };
        }

        #endregion IOperateRight 成员

        public string WfPrefix { get; set; }

        public bool ReadOnly { get; set; }
    }
}