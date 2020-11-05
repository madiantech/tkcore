using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Right
{
    public sealed class SimpleListObjectOperateRight : IObjectOperateRight
    {
        private readonly UpdateKind fOperators;

        public SimpleListObjectOperateRight(UpdateKind operators)
        {
            fOperators = operators;
        }

        #region IObjectOperateRight 成员

        public ObjectOperatorCollection GetOperator(ObjectOperateRightEventArgs e)
        {
            if (e.Style.Style == PageStyle.List)
            {
                if (e.MainObj == null)
                {
                    if (fOperators.Contains(UpdateKind.Insert))
                        return new ObjectOperatorCollection(RightConst.INSERT);
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
                    return new ObjectOperatorCollection(result);
                }
            }

            return null;
        }

        #endregion
    }
}
