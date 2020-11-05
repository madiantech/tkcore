using System;
using System.Collections.Generic;
using System.Threading;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Threading
{
    public class WorkerThread : IDisposable
    {
        private readonly Thread fThreadObj;
        private bool fEndLoop;
        private readonly Mutex fEndLoopMutex;
        private readonly AutoResetEvent fItemAdded;
        private readonly Queue<WorkItem> fWorkItemQueue;

        public WorkerThread()
        {
            fEndLoopMutex = new Mutex();
            fItemAdded = new AutoResetEvent(false);
            fWorkItemQueue = new Queue<WorkItem>();
            fThreadObj = new Thread(Run);
        }

        public WorkerThread(bool autoStart)
            : this()
        {
            if (autoStart)
                Start();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        public int ManagedThreadId
        {
            get
            {
                return fThreadObj.ManagedThreadId;
            }
        }

        public Thread Thread
        {
            get
            {
                return fThreadObj;
            }
        }

        public bool EndLoop
        {
            get
            {
                bool result = false;
                fEndLoopMutex.WaitOne();
                result = fEndLoop;
                fEndLoopMutex.ReleaseMutex();
                return result;
            }
            set
            {
                fEndLoopMutex.WaitOne();
                fEndLoop = value;
                fEndLoopMutex.ReleaseMutex();
            }
        }

        public bool QueueEmpty
        {
            get
            {
                lock (fWorkItemQueue)
                {
                    return fWorkItemQueue.Count == 0;
                }
            }
        }

        public string Name
        {
            get
            {
                return fThreadObj.Name;
            }
            set
            {
                fThreadObj.Name = value;
            }
        }

        private void QueueWorkItem(WorkItem workItem)
        {
            lock (fWorkItemQueue)
            {
                fWorkItemQueue.Enqueue(workItem);
                fItemAdded.Set();
            }
        }

        private WorkItem GetNext()
        {
            if (QueueEmpty)
                return null;
            lock (fWorkItemQueue)
            {
                return fWorkItemQueue.Dequeue();
            }
        }

        private void Run()
        {
            while (!EndLoop)
            {
                while (!QueueEmpty)
                {
                    if (EndLoop)
                        return;
                    WorkItem workItem = GetNext();
                    using (workItem)
                    {
                        workItem.CallBack();
                    }
                }
                fItemAdded.WaitOne();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Kill();
            }
        }

        public override int GetHashCode()
        {
            return fThreadObj.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return fThreadObj.Equals(obj);
        }

        public void Start()
        {
            TkDebug.AssertNotNull(fThreadObj, "线程对象没有初始化", this);
            TkDebug.Assert(!fThreadObj.IsAlive, "当前工作线程已经激活，不能再调用Start", this);

            fThreadObj.Start();
        }

        public void Kill()
        {
            //Kill is called on client thread - must use cached thread object
            TkDebug.AssertNotNull(fThreadObj, "线程对象没有初始化", this);
            //Debug.Assert(m_ThreadObj != null);
            if (!fThreadObj.IsAlive)
                return;
            EndLoop = true;
            fItemAdded.Set();

            //Wait for thread to die
            fThreadObj.Join();

            fItemAdded.Close();
            fEndLoopMutex.Close();
            fWorkItemQueue.Clear();
        }

        public IAsyncResult BeginInvoke(AsyncCallback callback, Delegate method,
            object[] args, object asyncState)
        {
            TkDebug.AssertArgumentNull(method, "method", this);

            WorkItem result = new WorkItem(callback, asyncState, method, args);
            QueueWorkItem(result);
            return result;
        }

        public object EndInvoke(IAsyncResult result)
        {
            TkDebug.AssertArgumentNull(result, "result", this);
            WorkItem workItem = result.Convert<WorkItem>();

            result.AsyncWaitHandle.WaitOne();
            return workItem.MethodReturnedValue;
        }

        //public static WorkerThread SysThread
        //{
        //    get
        //    {
        //        TkDebug.ThrowIfNoGlobalVariable();
        //        WorkerThread result = AbstractGlobalVariable.Current.WorkThread;
        //        TkDebug.AssertNotNull(result, "GlobalVariable的WorkThread没有实现，无法使用", null);
        //        return result;
        //    }
        //}

        public override string ToString()
        {
            return base.ToString();
        }
    }
}