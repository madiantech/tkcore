using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;
using YJC.Toolkit.Data;
using System.Data;


namespace YJC.Toolkit.SysFunction
{
    [ModuleCreator(Author = "YJC", CreateDate = "2015-10-19",
        Description = "创建对指定代码表进行数据维护的Module")]
    internal class CodeTableModuleCreator : IModuleCreator
    {
        #region IModuleCreator 成员

        public IModule Create(string source)
        {
            using (EmptyDbDataSource dbSource = new EmptyDbDataSource())
            using (MaintainCodetableResolver resolver = new MaintainCodetableResolver(dbSource))
            {
                source = source.Replace('/', '_');
                DataRow row = resolver.TrySelectRowWithParam("TableName", source);
                if (row != null)
                    return new CodeTableModule(row);
                else 
                    throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                        "表SYS_MAINTAIN_CODETABLE缺少{0}的配置", source), this);
            }
        }

        #endregion
    }
}
