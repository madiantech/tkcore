using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtmlCreator(ControlHtmlCreatorConst.TK55_VUE_MODEL, RegName = ControlConst.TIME)]
    internal class TimeVueControlHtmlCreator : BaseVueControlHtmlCreator
    {
        public override string CreateHtml(string tableName, Tk5FieldInfoEx field,
            IFieldValueProvider provider, bool needId)
        {
            throw new NotImplementedException();
        }
    }
}