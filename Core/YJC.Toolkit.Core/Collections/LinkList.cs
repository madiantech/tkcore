using System;
using System.Collections;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Collections
{
    internal class LinkList<T> : IEnumerable<T>
    {
        internal class InternalEnumerator : IEnumerator<T>
        {
            private ListNode<T> fCurrent;
            private readonly LinkList<T> fList;

            public InternalEnumerator(LinkList<T> list)
            {
                fList = list;
            }

            #region IEnumerator<T> 成员

            public T Current
            {
                get
                {
                    return fCurrent.Value;
                }
            }

            #endregion

            #region IDisposable 成员

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator 成员

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public bool MoveNext()
            {
                if (fCurrent == null)
                    fCurrent = fList.Head;
                else
                    fCurrent = fCurrent.Next;
                return fCurrent != null;
            }

            public void Reset()
            {
                fCurrent = null;
            }

            #endregion
        }

        #region 变量字段声明

        //当前节点个数
        private int fCount;

        //头结点
        private ListNode<T> fHead;

        //未节点
        private ListNode<T> fTail;

        #endregion

        #region IEnumerable<T> 成员

        public IEnumerator<T> GetEnumerator()
        {
            return new InternalEnumerator(this);
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region 属性声明

        public int Count
        {
            get
            {
                return fCount;
            }
        }

        public ListNode<T> Head
        {
            get
            {
                return fHead;
            }
        }

        public ListNode<T> Tail
        {
            get
            {
                return fTail;
            }
        }

        #endregion

        #region 判断链表处理异常方法

        /// <summary>
        /// 判断常规异常
        /// </summary>
        /// <param>链表是空的</param>
        /// <param>索引过小</param>
        /// <param >索引过大</param>
        private void JudgeNormalException(int index)
        {
            TkDebug.AssertNotNull(fHead, "链表是空的。", this);
            //Assert的bool值参数如果返回false则报错！
            TkDebug.Assert(index >= 0 && index < fCount, "索引超出链表范围。", this);
        }

        #endregion

        #region 添加特殊情况添加首节点尾节点

        /// <summary>
        /// 链表大小
        /// </summary>        
        /// <summary>
        /// 添加节点到链表的开头
        /// </summary>
        /// <param name="value">要添加的数据</param>
        private void AddFirst(ListNode<T> newNode)
        {
            //如果头为null
            if (fHead == null)
            {
                //把头节点设置为node
                fHead = newNode;
                //因为是空链表，所以头尾一致
                fTail = newNode;
            }
            else
            {
                //原来头节点的上一个为新节点
                fHead.Prev = newNode;
                //新节点的下一个为原来的头节点
                newNode.Next = fHead;
                //新头节点为新节点
                fHead = newNode;
            }
            //大小加一
            fCount++;
        }

        public ListNode<T> AddFirst(T value)
        {
            ListNode<T> node = new ListNode<T>(this, value);
            AddFirst(node);
            return node;
        }
        /// <summary>
        /// 添加节点到链表的末尾
        /// </summary>
        /// <param name="value">要添加的数据</param>
        private void AddLast(ListNode<T> current)
        {
            if (fHead == null)
            {
                //把头节点设置为node
                fHead = current;
                //因为是空链表，所以头尾一致
                fTail = current;
            }
            else
            {
                //将原尾节点的下一个设置为新节点
                fTail.Next = current;
                //将新节点的上一个设置为原尾节点
                current.Prev = fTail;
                //将尾节点重新设置为新节点
                fTail = current;
            }
            //大小加一
            fCount++;
        }

        public ListNode<T> AddLast(T value)
        {
            ListNode<T> node = new ListNode<T>(this, value);
            AddLast(node);
            return node;
        }

        #endregion

        #region 删除特殊情况移除首节点尾节点

        /// <summary>
        /// 移除头节点
        /// </summary>
        private void RemoveFirst()
        {
            //如果fCount为1，那就是清空链表。
            if (fCount == 1)
                Clear();
            else
            {
                //将头节点设为原头结点的下一个节点，就是下一个节点上移
                fHead = fHead.Next;
                //处理上一步遗留问题，原来的第二个节点的上一个是头结点，现在第二个要变成头节点，那要把它的Prev设为null才能成为头节点
                fHead.Prev = null;
                //大小减一
                fCount--;
            }
        }

        /// <summary>
        /// 移除尾节点
        /// </summary>
        private void RemoveLast()
        {
            //如果fCount为1，那就是清空链表。
            if (fCount == 1)
                Clear();
            else
            {
                //尾节点设置为倒数第二个节点
                fTail = fTail.Prev;
                //将新尾节点的Next设为null，表示它是新的尾节点
                fTail.Next = null;
                //大小减一
                fCount--;
            }
        }

        #endregion

        #region 检测当前节点是否包含在链表中

        private void JudgeListContainCurrentException(ListNode<T> current)
        {
            TkDebug.AssertNotNull(fHead, "该链表为空！", this);
            //判断当前节点是否为当前链表中
            //传送LinkList<T>对象给ListNode<T>构造函数，是为了确定不同的节点对应到各自不同的类节点链表里面
            //判断当前节点是否在this链表中
            TkDebug.Assert(current.List == this, "您插入的当前链表不匹配！", this);
        }

        #endregion

        #region 通过索引获得节点

        /// <summary>
        /// 通过索引获得节点
        /// </summary>
        /// <param name="index">索引值</param>
        /// <returns></returns>
        public ListNode<T> GetIndexNode(int index)
        {
            JudgeNormalException(index);
            ListNode<T> current;
            //如果索引在前一半，那么从前向后找
            if (index < (fCount >> 1))
            {
                current = fHead;
                for (int i = 0; i < index; ++i)
                    current = current.Next;
            }
            else//如果索引在后一半，那么从后向前找
            {
                current = fTail;
                for (int i = fCount - 1; i > index; --i)
                    current = current.Prev;
            }
            return current;
        }

        #endregion

        /// <summary>
        /// 清除链表中的数据
        /// </summary>
        public void Clear()
        {
            fHead = null;
            fTail = null;
            fCount = 0;
        }

        /// <summary>
        /// 根据索引获取链表中的节点
        /// </summary>
        /// <param name="index">整型索引</param>
        /// <returns>节点</returns>
        public T this[int index]
        {
            get
            {
                return GetIndexNode(index).Value;
            }
            set
            {
                GetIndexNode(index).Value = value;
            }
        }

        /// <summary>
        /// 在给定的节点处之前插入数据
        /// </summary>
        /// <param name="index">节点</param>
        /// <param name="value">要插入的数据</param>
        public ListNode<T> AddBefore(ListNode<T> node, T value)
        {
            ListNode<T> newNode = new ListNode<T>(this, value);
            //如果要添加的是头节点
            if (node == fHead)
                AddFirst(newNode);
            else
            {
                //当前节点的上一个的下一个设置为新节点
                node.Prev.Next = newNode;
                //新节点的上一个设置为当前节点的上一个
                newNode.Prev = node.Prev;
                //新节点的下一个设置为当前节点
                newNode.Next = node;
                //当前节点的上一个设置为新节点
                node.Prev = newNode;
                //大小加一
                fCount++;
            }
            return newNode;
        }

        /// <summary>
        /// 在给定的节点处之后插入数据
        /// </summary>
        /// <param name="index">节点</param>
        /// <param name="value">要插入的数据</param>
        public ListNode<T> AddAfter(ListNode<T> node, T value)
        {
            ListNode<T> newNode = new ListNode<T>(this, value);
            //如果要添加的是尾节点
            if (node == fTail)
                AddLast(newNode);
            else
            {
                //新节点的上一个节点设置为当前节点
                newNode.Prev = node;
                //新节点的下一个节点设置为当前节点的下一个节点
                newNode.Next = node.Next;
                //当前节点的下一个节点的上一个节点设置为新节点
                node.Next.Prev = newNode;
                //当前节点的下一个节点设置为新节点
                node.Next = newNode;
                //大小加一
                fCount++;
            }
            return newNode;
        }

        /// <summary>
        /// 移除链表中的节点
        /// </summary>
        /// <param name="index">要移除的节点的节点</param>
        public void Remove(ListNode<T> node)
        {
            //如果要移除的是头节点
            if (node == fHead)
            {
                RemoveFirst();
                return;
            }
            //如果要移除的是未节点
            if (node == fTail)
            {
                RemoveLast();
                return;
            }
            //当前节点的上一个的Next设置为当前节点的Next
            node.Prev.Next = node.Next;
            //当前节点的下一个的Prev设置为当前节点的Prev
            node.Next.Prev = node.Prev;
            //大小减一
            fCount--;
        }

        public void CopyTo(Array array, int index)
        {
            ListNode<T> head = fHead;
            if (head != null)
            {
                do
                {
                    array.SetValue(head.Value, index++);
                    head = head.Next;
                }
                while (head != null);
            }
        }

        /// <summary>
        /// 通过节点返回索引值
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int IndexOf(ListNode<T> node)
        {
            JudgeListContainCurrentException(node);
            //int currentCount=0;
            if (node == fTail)
                return fCount - 1;
            else
            {
                ListNode<T> current = fHead;
                for (int i = 0; i < fCount; ++i)
                {
                    if (node == current)
                        return i;
                    current = current.Next;
                }
                return -1;
            }
        }
    }
}
