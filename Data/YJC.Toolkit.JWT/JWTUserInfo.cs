using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class JWTUserInfo : IUserInfo, IReadObjectCallBack, IHttpHeader
    {
        public const string JWT_HEADER = "Authorization";
        private const int PAGE_SIZE = 15;

        public JWTUserInfo()
        {
            IsLogOn = false;
            PageSize = PAGE_SIZE;
        }

        public JWTUserInfo(string userName, string logOnName, object userId, object mainOrgId)
        {
            UserName = userName;
            LogOnName = logOnName;
            UserId = userId;
            MainOrgId = mainOrgId;
            IsLogOn = true;
            PageSize = PAGE_SIZE;
            UseRequestHeader = false;
            ValidTime = JWTUtil.CalcValidTime();
        }

        public JWTUserInfo(string userName, string logOnName, object userId,
            object mainOrgId, IEnumerable<string> roleIds)
            : this(userName, logOnName, userId, mainOrgId)
        {
            InternalRoleIds = new List<string>();
            InternalRoleIds.AddRange(roleIds);
        }

        public JWTUserInfo(IUserInfo info)
        {
            if (info == null)
            {
                IsLogOn = false;
                PageSize = PAGE_SIZE;
            }
            else
            {
                UserName = info.UserName;
                LogOnName = info.LogOnName;
                UserId = info.UserId;
                MainOrgId = info.MainOrgId;
                PageSize = info.PageSize;
                if (info.RoleIds != null)
                {
                    InternalRoleIds = new List<string>();
                    foreach (var item in info.RoleIds)
                        if (item != null)
                            InternalRoleIds.Add(item.ToString());
                }
                IsLogOn = info.IsLogOn;
                UseRequestHeader = false;
                if (IsLogOn)
                    ValidTime = JWTUtil.CalcValidTime();
            }
        }

        [SimpleAttribute]
        public string UserName { get; protected set; }

        [SimpleAttribute]
        public string LogOnName { get; protected set; }

        //public string Encoding { get; private set; }

        [SimpleAttribute(ObjectType = typeof(string))]
        public object UserId { get; protected set; }

        [SimpleAttribute(ObjectType = typeof(string))]
        public object MainOrgId { get; protected set; }

        public IEnumerable OtherOrgs
        {
            get
            {
                return null;
            }
        }

        public IEnumerable RoleIds
        {
            get
            {
                return InternalRoleIds;
            }
        }

        [SimpleAttribute]
        public int PageSize { get; set; }

        public bool IsLogOn { get; protected set; }

        public object Data1 { get; set; }

        public object Data2 { get; set; }

        [SimpleElement(IsMultiple = true, LocalName = "RoleId")]
        private List<string> InternalRoleIds { get; set; }

        [SimpleAttribute]
        public DateTime ValidTime { get; protected set; }

        public string HeaderName => JWT_HEADER;

        public bool UseRequestHeader { get; private set; }

        public string Token
        {
            get
            {
                return JWTUtil.EncodeToJwt(this);
            }
        }

        public void OnReadObject()
        {
            UseRequestHeader = !NeedResetTime();
            if (!UseRequestHeader)
                ValidTime = JWTUtil.CalcValidTime();
            IsLogOn = !string.IsNullOrEmpty(UserId.ConvertToString());
        }

        private bool NeedResetTime()
        {
            DateTime date = DateTime.Now.AddHours(1);
            return ValidTime <= date;
        }
    }
}