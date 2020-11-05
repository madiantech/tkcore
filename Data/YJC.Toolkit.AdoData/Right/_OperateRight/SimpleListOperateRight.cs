using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class SimpleListOperateRight : IOperateRight
    {
        private readonly UpdateKind fOperators;

        public SimpleListOperateRight(UpdateKind operators)
        {
            fOperators = operators;
        }

        #region IOperateRight 成员

        public IEnumerable<string> GetOperator(OperateRightEventArgs e)
        {
            if (e.Style.Style == PageStyle.List)
            {
                if (e.Row == null)
                {
                    if (fOperators.Contains(UpdateKind.Insert))
                        return EnumUtil.Convert(RightConst.INSERT);
                    return null;
                }
                else
                {
                    List<string> result = new List<string>(2);
                    if (fOperators.Contains(UpdateKind.Update))
                        result.Add(RightConst.UPDATE);
                    if (fOperators.Contains(UpdateKind.Delete))
                        result.Add(RightConst.DELETE);
                    if (result.Count == 0)
                        return null;
                    return result;
                }
            }

            return null;
        }

        #endregion
    }
}
