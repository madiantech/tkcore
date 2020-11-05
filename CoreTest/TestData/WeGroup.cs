using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.User
{
    [TypeScheme(Author = "YJC", CreateDate = "2014-11-12", Description = "微信用户组信息的元数据")]
    public class WeGroup : IEntity, IRegName
    {
        internal WeGroup()
        {
        }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return Id.ToString(ObjectUtil.SysCulture);
            }
        }

        #endregion IRegName 成员

        #region IEntity 成员

        string IEntity.Id
        {
            get
            {
                return RegName;
            }
        }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 20)]
        [FieldInfo(Length = 255)]
        [FieldControl(ControlType.Text, Order = 20)]
        [FieldLayout(FieldLayout.PerLine)]
        [DisplayName("组名")]
        public string Name { get; set; }

        #endregion IEntity 成员

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 10)]
        [FieldInfo(IsKey = true)]
        [FieldControl(ControlType.Hidden, Order = 10)]
        public int Id { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 30)]
        [FieldControl(ControlType.Label, Order = 30, DefaultShow = PageStyle.List)]
        [DisplayName("人数")]
        public int? Count { get; private set; }

        [SimpleAttribute]
        [DisplayName("新密码")]
        [FieldControl(ControlType.Password, Order = 50)]
        [FieldLayout(FieldLayout.PerLine)]
        [FieldInfo(IsEmpty = false, Length = 30)]
        public string Password { get; protected set; }

        [SimpleAttribute]
        [DisplayName("确认密码")]
        [FieldControl(ControlType.Password, Order = 60)]
        [FieldLayout(FieldLayout.PerLine)]
        [FieldInfo(IsEmpty = false, Length = 30)]
        public string ConfirmPassword { get; protected set; }

        public string DisplayName
        {
            get
            {
                return Name;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}