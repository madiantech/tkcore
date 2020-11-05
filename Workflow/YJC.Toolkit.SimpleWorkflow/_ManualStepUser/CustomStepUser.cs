using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class CustomStepUser : IManualStepUser
    {
        private readonly string fExpression;

        public CustomStepUser(string expression)
        {
            TkDebug.AssertArgumentNullOrEmpty(expression, "expression", null);

            fExpression = expression;
            Format = CustomFormat.SingleUser;
        }

        #region IManualStepUser 成员

        public IEnumerable<string> GetUserList(WorkflowContent content,
            DataRow workflowRow, IDbDataSource source)
        {
            string result = StepUtil.ExecuteExpression<string>(fExpression,
                content.MainRow, workflowRow);

            switch (Format)
            {
                case CustomFormat.SingleUser:
                    return EnumUtil.Convert(result);

                case CustomFormat.QuoteStringList:
                    return result.Value<QuoteStringList>().CreateEnumerable();

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return null;
            }
        }

        #endregion IManualStepUser 成员

        public CustomFormat Format { get; set; }
    }
}