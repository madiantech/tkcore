using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.IM;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    [TypeScheme(Author = "YJC", CreateDate = "2014-11-20", Description = "微信企业号的组织部门的元数据")]
    public class Department : BaseResult, IEntity, IRegName, ITreeNode, IDecoderItem, IOrder
    {
        protected Department()
        {
        }

        public Department(string name, int parentId, int order)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", null);

            Name = name;
            ParentId = parentId;
            Order = order;
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

        #region ITreeNode 成员

        public bool HasChildren
        {
            get
            {
                return false;
            }
        }

        public bool HasParent
        {
            get
            {
                return ParentId != 0;
            }
        }

        string ITreeNode.ParentId
        {
            get
            {
                return ParentId.ToString(ObjectUtil.SysCulture);
            }
        }

        ITreeNode ITreeNode.Parent
        {
            get
            {
                //var collection = GetDepartmentCollection();
                //return collection.GetParent(ParentId);
                return null;
            }
        }

        IEnumerable<ITreeNode> ITreeNode.Children
        {
            get
            {
                //var collection = GetDepartmentCollection();
                //return collection.GetChildren(Id);
                return null;
            }
        }

        #endregion ITreeNode 成员

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 10)]
        [FieldInfo(IsKey = true)]
        [FieldControl(ControlType.Hidden, Order = 10)]
        public int Id { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 20)]
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

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 30)]
        [FieldControl(ControlType.Label, Order = 30)]
        [FieldLayout(FieldLayout.PerLine)]
        [FieldDecoder(DecoderType.EasySearch, "CorpDepartment")]
        [DisplayName("父节点")]
        public int ParentId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 40)]
        public int Order { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}