using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.IM;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    [TypeScheme(Author = "YJC", CreateDate = "2014-11-19", Description = "微信企业号的标签信息")]
    public class Tag : BaseResult, IEntity, IRegName, IDecoderItem
    {
        public Tag()
        {
        }

        public Tag(int tagId)
        {
            Id = tagId;
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

        #endregion IEntity 成员

        #region IDecoderItem 成员

        string IDecoderItem.Value
        {
            get
            {
                return RegName;
            }
        }

        string IDecoderItem.this[string name]
        {
            get
            {
                return null;
            }
        }

        #endregion IDecoderItem 成员

        [SimpleElement(LocalName = "tagid", Order = 10)]
        [FieldInfo(IsKey = true)]
        [FieldControl(ControlType.Hidden, Order = 10)]
        public int Id { get; set; }

        [SimpleElement(LocalName = "tagname", Order = 10)]
        [FieldInfo(Length = 255)]
        [FieldControl(ControlType.Text, Order = 20)]
        [FieldLayout(FieldLayout.PerLine)]
        [DisplayName("名称")]
        public string Name { get; set; }

        public string DisplayName
        {
            get
            {
                return Name;
            }
        }
    }
}