using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public class QuoteStringListSearch : BaseListSearch
    {
        public override IParamBuilder GetCondition(IFieldInfo field, string fieldValue)
        {
            string likeValue = string.Format(ObjectUtil.SysCulture, "%\"{0}\"%", fieldValue);
            return SqlParamBuilder.CreateSingleSql(Context, field, "LIKE", likeValue);
        }
    }
}