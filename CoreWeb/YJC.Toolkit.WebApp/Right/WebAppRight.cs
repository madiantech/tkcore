using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class WebAppRight
    {
        private IFunctionRight fFunctionRight;
        private ILogOnRight fLogOnRight;
        private IMenuScriptBuilder fScriptBuilder;
        private string fMenu;

        public WebAppRight(IAppRightBuilder builder)
        {
            TkDebug.AssertArgumentNull(builder, nameof(builder), null);

            var empty = EmptyAppRightBuilder.Instance;
            fLogOnRight = builder.CreateLogOnRight() ?? empty.CreateLogOnRight();
            fFunctionRight = builder.CreateFunctionRight() ?? empty.CreateFunctionRight();
            fScriptBuilder = builder.CreateScriptBuilder() ?? empty.CreateScriptBuilder();
        }

        public ILogOnRight LogOnRight
        {
            get
            {
                return fLogOnRight;
            }
            set
            {
                TkDebug.AssertArgumentNull(value, "value", this);
                fLogOnRight = value;
            }
        }

        public IFunctionRight FunctionRight
        {
            get
            {
                return fFunctionRight;
            }
            set
            {
                TkDebug.AssertArgumentNull(value, "value", this);
                fFunctionRight = value;
            }
        }

        public IMenuScriptBuilder ScriptBuilder
        {
            get
            {
                return fScriptBuilder;
            }
            set
            {
                TkDebug.AssertArgumentNull(value, "value", this);
                fScriptBuilder = value;
            }
        }

        public void Initialize(IUserInfo user)
        {
            TkDebug.AssertArgumentNull(user, "user", this);

            BaseGlobalVariable.Current.UserInfo = user;
            LogOnRight.Initialize(user);
            FunctionRight.Initialize(user);
        }

        public string CreateMenu(IUserInfo user)
        {
            if (fMenu == null)
            {
                DataSet dataSet = FunctionRight.GetMenuObject(user.UserId);
                if (dataSet != null)
                {
                    using (dataSet)
                    {
                        fMenu = ScriptBuilder.GetMenuScript(dataSet);
                    }
                }
                if (fMenu == null)
                    fMenu = string.Empty;
            }
            return fMenu;
        }
    }
}