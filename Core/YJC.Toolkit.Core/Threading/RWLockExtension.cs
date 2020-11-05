using System;
using System.Threading;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Threading
{
    public static class RWLockExtension
    {
        public const int TIME_OUT = 10;

        public static T ReadLockAction<T>(this ReaderWriterLockSlim rwlock, Func<T> action)
        {
            TkDebug.AssertArgumentNull(rwlock, nameof(rwlock), null);
            TkDebug.AssertArgumentNull(action, nameof(action), null);

            rwlock.EnterUpgradeableReadLock();
            try
            {
                return action();
            }
            finally
            {
                rwlock.ExitUpgradeableReadLock();
            }
        }

        public static (bool Result, T Value) TryReadLockAction<T>(this ReaderWriterLockSlim rwlock,
            Func<T> action)
        {
            TkDebug.AssertArgumentNull(rwlock, nameof(rwlock), null);
            TkDebug.AssertArgumentNull(action, nameof(action), null);

            try
            {
                if (rwlock.TryEnterUpgradeableReadLock(TIME_OUT))
                {
                    try
                    {
                        var value = action();
                        return (true, value);
                    }
                    finally
                    {
                        rwlock.ExitUpgradeableReadLock();
                    }
                }
            }
            catch
            {
            }
            return (false, default);
        }

        public static T WriteLockAction<T>(this ReaderWriterLockSlim rwlock, Func<T> action)
        {
            TkDebug.AssertArgumentNull(rwlock, nameof(rwlock), null);
            TkDebug.AssertArgumentNull(action, nameof(action), null);

            rwlock.EnterWriteLock();
            try
            {
                return action();
            }
            finally
            {
                rwlock.ExitWriteLock();
            }
        }

        public static (bool Result, T Value) TryWriteLockAction<T>(this ReaderWriterLockSlim rwlock,
            Func<T> action)
        {
            TkDebug.AssertArgumentNull(rwlock, nameof(rwlock), null);
            TkDebug.AssertArgumentNull(action, nameof(action), null);

            if (rwlock.TryEnterWriteLock(TIME_OUT))
            {
                try
                {
                    var value = action();
                    return (true, value);
                }
                finally
                {
                    rwlock.ExitWriteLock();
                }
            }
            return (false, default);
        }
    }
}