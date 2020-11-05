using YJC.Toolkit.Data;
using YJC.Toolkit.LogOn;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    abstract class BasePasswordUpdateSource : IUpdateObjectSource
    {
        protected BasePasswordUpdateSource()
        {
        }

        #region IObjectUpdateSource 成员

        public OutputData Update(IInputData input, object instance)
        {
            BasePasswordData passwd = instance.Convert<BasePasswordData>();

            using (EmptyDbDataSource source = new EmptyDbDataSource())
            using (UserResolver resolver = new UserResolver(source))
            {
                return ChangePasswd(resolver, passwd);
            }
        }

        #endregion

        #region IObjectDetailSource 成员

        public abstract object Query(IInputData input, string id);

        #endregion

        protected abstract OutputData ChangePasswd(UserResolver resolver, BasePasswordData passwd);
    }
}
