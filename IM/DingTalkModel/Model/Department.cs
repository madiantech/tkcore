using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.IM;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model
{
    [TypeScheme(Author = "YJC", CreateDate = "2014-11-20", Description = "微信企业号的组织部门的元数据")]
    public class Department : BaseResult, IEntity, IRegName, ITreeNode, IDecoderItem, IOrder
    {
        internal Department()
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
        public int Id { get; private set; }

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
        public int ParentId { get; internal set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 40)]
        public int Order { get; set; }

        public override string ToString()
        {
            return Name;
        }

        //public bool Delete()
        //{
        //    string url = string.Format(ObjectUtil.SysCulture, WeCorpConst.DELETE_DEPARTMENT,
        //        CorpAccessToken.GetTokenWithSecret(WeixinSettings.Current.CorpUserManagerSecret), Id);
        //    WeixinResult result = WeUtil.GetFromUri<WeixinResult>(url);
        //    return !result.IsError;
        //}

        //internal void VerifyOrder()
        //{
        //    if (Order == 0)
        //        Order = Id;
        //}

        //public WeixinResult Update()
        //{
        //    string url = WeCorpUtil.GetCorpUrl(WeCorpConst.UPDATE_DEPARTMENT,
        //        WeixinSettings.Current.CorpUserManagerSecret);
        //    return WeUtil.PostToUri(url, this.WriteJson(WeConst.WRITE_SETTINGS), new WeixinResult());
        //}

        //public CorpUserList GetAllUsers(bool fetchChild)
        //{
        //    return GetUsers(Id, fetchChild, true, UserStatus.Attention);
        //}

        //public CorpUserList GetUsers(bool fetchChild, UserStatus status)
        //{
        //    return GetUsers(Id, fetchChild, false, status);
        //}

        //public override string ToString()
        //{
        //    return Name;
        //}

        //private static CorpUserList GetUsers(int id, bool fetchChild, bool allUser, UserStatus status)
        //{
        //    int userStatus = allUser ? 0 : (int)status;
        //    int fetch = fetchChild ? 1 : 0;
        //    string url = string.Format(ObjectUtil.SysCulture, WeCorpConst.QUERY_DEPARTMENT_USER,
        //        CorpAccessToken.GetTokenWithSecret(WeixinSettings.Current.CorpUserManagerSecret),
        //        id, fetch, userStatus);

        //    return WeUtil.GetFromUri(url, new CorpUserList());
        //}

        //public static CorpDepartment Create(int parentId, string name, int order)
        //{
        //    TkDebug.AssertArgumentNullOrEmpty(name, "name", null);

        //    CorpDepartment data = new CorpDepartment
        //    {
        //        Name = name,
        //        ParentId = parentId,
        //        Order = order
        //    };
        //    string url = WeCorpUtil.GetCorpUrl(WeCorpConst.CREATE_DEPARTMENT,
        //        WeixinSettings.Current.CorpUserManagerSecret);
        //    return WeUtil.PostToUri(url, data.WriteJson(WeConst.WRITE_SETTINGS), data);
        //}

        //internal static CorpDepartmentCollection GetDepartmentList()
        //{
        //    string url = WeCorpUtil.GetCorpUrl(WeCorpConst.QUERY_DEPARTMENT,
        //        WeixinSettings.Current.CorpUserManagerSecret);
        //    CorpDepartmentCollection collection = WeUtil.GetFromUri<CorpDepartmentCollection>(url);
        //    return collection;
        //}

        //public static IList<CorpDepartment> GetAllDepartments()
        //{
        //    CorpDepartmentCollection collection = GetDepartmentList();
        //    return collection.Department;
        //}

        //public static CorpUserList GetAllUsers(int id, bool fetchChild)
        //{
        //    return GetUsers(id, fetchChild, true, UserStatus.Attention);
        //}

        //public static CorpUserList GetUsers(int id, bool fetchChild, UserStatus status)
        //{
        //    return GetUsers(id, fetchChild, false, status);
        //}

        //public static CorpDetailUserList GetDetailUsers(int id, bool fetchChild, bool allUser, UserStatus status)
        //{
        //    int userStatus = allUser ? 0 : (int)status;
        //    int fetch = fetchChild ? 1 : 0;
        //    string url = string.Format(ObjectUtil.SysCulture, WeCorpConst.QUERY_DEPARTMENT_DETAIL_USER,
        //        CorpAccessToken.GetTokenWithSecret(WeixinSettings.Current.CorpUserManagerSecret),
        //        id, fetch, userStatus);

        //    return WeUtil.GetFromUri(url, new CorpDetailUserList());
        //}

        //private static CorpDepartmentCollection GetDepartmentCollection()
        //{
        //    return CacheManager.GetItem("WeixinListData", WeCorpConst.CORP_DEPT_NAME)
        //        .Convert<CorpDepartmentCollection>();
        //}
    }
}