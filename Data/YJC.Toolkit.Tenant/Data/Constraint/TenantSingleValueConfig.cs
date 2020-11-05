using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2018-09-13",
        Author = "YJC", Description = "在租户模式下，数据表中该字段在同一租户下必须唯一的校验")]
    [ObjectContext]
    internal class TenantSingleValueConfig : IConfigCreator<BaseConstraint>
    {
        #region IConfigCreator<BaseConstraint> 成员

        public BaseConstraint CreateObject(params object[] args)
        {
            TenantSingleValueConstraint result = new TenantSingleValueConstraint(
                ConstraintUtil.GetFieldInfo(args), ConstraintUtil.GetFieldInfo(TenantIdNickName, args));
            if (Message != null)
                result.Message = Message.ToString();

            return result;
        }

        #endregion IConfigCreator<BaseConstraint> 成员

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText Message { get; private set; }

        [SimpleAttribute(DefaultValue = TenantUtil.TENANT_NAME)]
        public string TenantIdNickName { get; private set; }
    }
}