using System;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Threading;

namespace YJC.Toolkit.Web
{
    public static class SessionUtil
    {
        public static T ReadSession<T>(string sessionName) where T : class
        {
            TkDebug.AssertArgumentNullOrEmpty(sessionName, "sessionName", null);

            IRWLock rwLock = WebGlobalVariable.SessionGbl.RWLock;
            T result = RWLock.ReadLockAction(rwLock, () => WebGlobalVariable.Session[sessionName] as T);
            return result;
        }

        public static void WriteSession(string sessionName, object data)
        {
            TkDebug.AssertArgumentNullOrEmpty(sessionName, "sessionName", null);
            TkDebug.AssertArgumentNull(data, "data", null);

            IRWLock rwLock = WebGlobalVariable.SessionGbl.RWLock;
            RWLock.WriteLockAction(rwLock, () =>
            {
                WebGlobalVariable.Session[sessionName] = data;
                return data;
            });
        }

        public static T ReadWriteSession<T>(string sessionName, Func<object[], T> createFunc,
            params object[] args) where T : class
        {
            TkDebug.AssertArgumentNullOrEmpty(sessionName, "sessionName", null);
            TkDebug.AssertArgumentNull(createFunc, "createFunc", null);

            IRWLock rwLock = WebGlobalVariable.SessionGbl.RWLock;
            T result = RWLock.ReadLockAction(rwLock, () => WebGlobalVariable.Session[sessionName] as T);
            if (result != null)
                return result;

            result = RWLock.WriteLockAction(rwLock, () =>
            {
                T data = createFunc(args);
                WebGlobalVariable.Session[sessionName] = data;
                return data;
            });
            return result;
        }

    }
}
