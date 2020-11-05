using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [Resolver(Author = "YJC", CreateDate = "2014-07-08",
        Description = "用户表(UR_USERS)的数据访问对象")]
    internal class UserResolver : Tk5TableResolver
    {
        public UserResolver(IDbDataSource source)
            : base("UserManager/User.xml", source)
        {
        }

        protected override void OnUpdatingRow(UpdatingEventArgs e)
        {
            base.OnUpdatingRow(e);

            switch (e.Status)
            {
                case UpdateKind.Insert:
                    e.Row["Id"] = CreateUniId();
                    e.Row["CreateId"] = e.Row["UpdateId"] = BaseGlobalVariable.UserId;
                    e.Row["CreateDate"] = e.Row["UpdateDate"] = DateTime.Now;
                    e.Row["Active"] = 1;
                    break;

                case UpdateKind.Update:
                    e.Row["UpdateId"] = BaseGlobalVariable.UserId;
                    e.Row["UpdateDate"] = DateTime.Now;
                    break;
            }
        }

        private IUserInfo ChangeUserState(DataRow rowUser, bool isValid)
        {
            IUserInfo result = null;
            SetCommands(AdapterCommand.Update);

            if (!isValid)
            {
                //strSQL = "UPDATE UR_USERS SET USER_ACTIVE = 0, USER_UNLOCK_DATE = {0} WHERE USER_ID ={1} ";
                //strSQL = string.Format(strSQL, Context.GetSqlParamName("USER_UNLOCK_DATE"), Context.GetSqlParamName("USER_ID"));
                //parameters.Add("USER_UNLOCK_DATE", XmlDataType.DateTime, DateTime.Now.AddDays(1));
                //parameters.Add("USER_ID", XmlDataType.String, rowUser["USER_ID"]);
            }
            else
            {
                rowUser.BeginEdit();
                try
                {
                    rowUser["Active"] = 1;
                    rowUser["LoginDate"] = DateTime.Now;
                    rowUser["UnlockDate"] = DBNull.Value;
                }
                finally
                {
                    rowUser.EndEdit();
                }

                result = new SimpleUserInfo(rowUser["Name"].ToString(),
                    rowUser["LoginName"].ToString(), rowUser["Id"], rowUser["OrgId"]);
            }
            UpdateDatabase();
            //result = new TenantUserInfo(result, 1);

            return result;
        }

        private static object CryptPassword(string passwd)
        {
            if (string.IsNullOrEmpty(passwd))
                return DBNull.Value;
            return CryptoUtil.Encrypt(passwd);
        }

        private static WebPostException CreateException(string message)
        {
            FieldErrorInfo field = new FieldErrorInfo("LogOnData", "LogOnName", message);
            return new WebPostException(message, field);
        }

        private static WebPostException CreateException()
        {
            return CreateException("用户名或者密码错误");
        }

        private bool ComparePassword(object srcPassword, object inputPassword)
        {
            if (srcPassword == DBNull.Value && inputPassword == DBNull.Value)
                return true;
            return srcPassword.ToString() == inputPassword.ToString();
        }

        private IUserInfo CheckUserLogOn(DataRow row, string pwd, int logonAttempts)
        {
            if (row == null)
                throw CreateException();

            DataRow user = row;

            //用户已离职
            if (user["Out"].Value<int>() == 1)
                throw CreateException("用户离职，请与系统管理员联系");

            //登录次数超过3次，锁定该帐号
            //if (logonAttempts >= LogOnParams.Current.MaxAttemptTimes)
            //{
            //    this.ChangeUserState(user, false, info);
            //    throw new UserLockedException();
            //}

            int active = user["Active"].Value<int>();
            if (active == 0)
            {
                if (user["UnlockDate"] == DBNull.Value)
                    throw CreateException("用户被锁定，请与系统管理员联系");

                DateTime now = DateTime.Now;
                DateTime unlockDate = user["UnlockDate"].Value<DateTime>();
                if (unlockDate >= now)
                    throw CreateException("用户被锁定，请与系统管理员联系");
                else
                {
                    if (ComparePassword(user["LoginPasswd"], CryptPassword(pwd)))
                        return ChangeUserState(user, true);
                    else
                        throw CreateException();
                }
            }
            else if (active == 1)
            {
                if (ComparePassword(user["LoginPasswd"], CryptPassword(pwd)))
                    return ChangeUserState(user, true);
                else
                    throw CreateException();
            }
            return null;
        }

        public bool ChangePassword(string id, string password, string oldPassword)
        {
            DataRow row = SelectRowWithKeys(id);
            if (oldPassword != null)
            {
                if (!ComparePassword(row["LoginPasswd"], CryptPassword(oldPassword)))
                    return false;
            }
            row["LoginPasswd"] = CryptPassword(password);
            SetCommands(AdapterCommand.Update);
            UpdateDatabase();
            return true;
        }

        public IUserInfo CheckUserLogOn(string logOnName, string pwd, int logOnAttempts)
        {
            DataRow row = TrySelectRowWithParam("LoginName", logOnName);
            return CheckUserLogOn(row, pwd, logOnAttempts);
        }

        public IUserInfo CheckUserLogOnById(string userId, string pwd)
        {
            DataRow row = TrySelectRowWithKeys(userId);
            return CheckUserLogOn(row, pwd, 0);
        }

        //public DataRow CheckUserLogOn(string orgId, string logOnName, string pwd, int logOnAttempts, UserInfo info)
        //{
        //    this.SelectWithParams(new string[] { "USER_ORG_ID", "USER_LOGIN_NAME" }, new object[] { orgId, logOnName });
        //    return this.CheckUserLogOn(pwd, logOnAttempts, info);
        //}

        //protected override void SetConstraints(PageStyle style)
        //{
        //    //this.Constraints.Add(new SingleValueConstraint("USER_LOGIN_NAME", this.GetDisplayName("USER_LOGIN_NAME"), XmlDataType.String));
        //    //this.Constraints.Add(new MobileConstraint("USER_MOBILE", this.GetDisplayName("USER_MOBILE")));
        //    //this.Constraints.Add(new EmailConstraint("USER_EMAIL", this.GetDisplayName("USER_EMAIL")));
        //    //this.Constraints.Add(new SFZConstraint("USER_IDENT_NO", this.GetDisplayName("USER_IDENT_NO")));
        //    //this.Constraints.Add(new PostCodeConstraint("USER_POSTAL", this.GetDisplayName("USER_POSTAL")));
        //}
    }
}