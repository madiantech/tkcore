using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;
using System.Transactions;

namespace YJC.Toolkit.SimpleRight
{
    [Source(Author = "YJC", CreateDate = "2015/07/30",
        Description = "设置角色的权限")]
    internal class PartRightSource : BaseDbSource
    {
        public override OutputData DoAction(IInputData input)
        {
            if (input.IsPost)
            {
                using (PartFuncResolver func = new PartFuncResolver(this) { UpdateMode = UpdateMode.DelIns })
                using (PartSubFuncResolver subFunc = new PartSubFuncResolver(this) { UpdateMode = UpdateMode.DelIns })
                {
                    string partId = input.QueryString["Id"];
                    DataSet postDataSet = input.PostObject.Convert<DataSet>();

                    func.SelectWithParam("PartId", partId);
                    subFunc.SelectWithParam("PartId", partId);

                    func.Update(postDataSet, input);
                    subFunc.Update(postDataSet, input);

                    UpdateUtil.UpdateTableResolvers(Context, (Action<Transaction>)null, func, subFunc);

                    return OutputData.CreateToolkitObject(KeyData.Empty);
                }
            }

            TkDebug.ThrowImpossibleCode(this);
            return null;
        }
    }
}