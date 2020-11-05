using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [OperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-11-02",
        Description = "根据表SYS_SUB_FUNC的数据并参考角色权限，再根据树的权限要求交叉获得的操作符配置")]
    internal class SubFuncTreeOperatorsConfig : BaseSubFuncOperatorsConfig, IReadObjectCallBack
    {
        private static readonly string[] DEFAULT_OPERS = new string[] {
            YJC.Toolkit.Right.RightConst.INSERT, YJC.Toolkit.Right.RightConst.UPDATE, 
            YJC.Toolkit.Right.RightConst.DELETE };

        public override IOperateRight CreateObject(params object[] args)
        {
            var totalOperators = Operators;
            TreeOperateRight right;
            if (totalOperators == null)
                right = new TreeOperateRight();
            else
            {
                var operators = (from config in Operators select config.Id).Except(DEFAULT_OPERS);
                right = new TreeOperateRight(operators);
            }
            right.DisableRootDelete = DisableRootDelete;
            right.LayerFieldName = LayerFieldName;
            return right;
        }

        [SimpleAttribute(DefaultValue = true)]
        public bool DisableRootDelete { get; internal set; }

        [SimpleAttribute(DefaultValue = DbTreeDefinition.LAYER_FIELD)]
        public string LayerFieldName { get; private set; }

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            Page = OperatorPage.Detail;
        }

        #endregion
    }
}
