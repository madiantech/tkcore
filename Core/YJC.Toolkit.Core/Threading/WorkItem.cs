using System;
using System.Threading;

namespace YJC.Toolkit.Threading
{
    internal class WorkItem : IAsyncResult, IDisposable
    {
        private readonly object[] fArgs;
        private readonly object fAsyncState;
        private readonly Delegate fMethod;
        private readonly ManualResetEvent fEvent;
        private readonly AsyncCallback fCallback;
        private bool fCompleted;
        private object fMethodReturnedValue;

        internal WorkItem(AsyncCallback callback, object asyncState, Delegate method, object[] args)
        {
            fAsyncState = asyncState;
            fMethod = method;
            fArgs = args;
            fCallback = callback;
            fEvent = new ManualResetEvent(false);
        }

        #region IAsyncResult 成员

        object IAsyncResult.AsyncState
        {
            get
            {
                return fAsyncState;
            }
        }

        WaitHandle IAsyncResult.AsyncWaitHandle
        {
            get
            {
                return fEvent;
            }
        }

        bool IAsyncResult.CompletedSynchronously
        {
            get
            {
                return false;
            }
        }

        bool IAsyncResult.IsCompleted
        {
            get
            {
                return Completed;
            }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            fEvent.Close();
            GC.SuppressFinalize(this);
        }

        #endregion

        public bool Completed
        {
            get
            {
                lock (this)
                {
                    return fCompleted;
                }
            }
            set
            {
                lock (this)
                {
                    fCompleted = value;
                }
            }
        }

        internal object MethodReturnedValue
        {
            get
            {
                lock (this)
                {
                    return fMethodReturnedValue;
                }
            }
            set
            {
                lock (this)
                {
                    fMethodReturnedValue = value;
                }
            }
        }

        //This method is called on the worker thread to execute the method
        internal void CallBack()
        {
            try
            {
                MethodReturnedValue = fMethod.DynamicInvoke(fArgs);
            }
            catch
            {
            }
            //Method is done. Signal the world
            fEvent.Set();
            if (fCallback != null)
            {
                try
                {
                    fCallback.DynamicInvoke(this);
                }
                catch
                {
                }
            }

            Completed = true;
        }
    }
}
