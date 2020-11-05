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

            #region IEnumerator<T> ��Ա

            public T Current
            {
                get
                {
                    return fCurrent.Value;
                }
            }

            #endregion

            #region IDisposable ��Ա

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator ��Ա

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

        #region �����ֶ�����

        //��ǰ�ڵ����
        private int fCount;

        //ͷ���
        private ListNode<T> fHead;

        //δ�ڵ�
        private ListNode<T> fTail;

        #endregion

        #region IEnumerable<T> ��Ա

        public IEnumerator<T> GetEnumerator()
        {
            return new InternalEnumerator(this);
        }

        #endregion

        #region IEnumerable ��Ա

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ��������

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

        #region �ж��������쳣����

        /// <summary>
        /// �жϳ����쳣
        /// </summary>
        /// <param>�����ǿյ�</param>
        /// <param>������С</param>
        /// <param >��������</param>
        private void JudgeNormalException(int index)
        {
            TkDebug.AssertNotNull(fHead, "�����ǿյġ�", this);
            //Assert��boolֵ�����������false�򱨴�
            TkDebug.Assert(index >= 0 && index < fCount, "������������Χ��", this);
        }

        #endregion

        #region ��������������׽ڵ�β�ڵ�

        /// <summary>
        /// �����С
        /// </summary>        
        /// <summary>
        /// ��ӽڵ㵽����Ŀ�ͷ
        /// </summary>
        /// <param name="value">Ҫ��ӵ�����</param>
        private void AddFirst(ListNode<T> newNode)
        {
            //���ͷΪnull
            if (fHead == null)
            {
                //��ͷ�ڵ�����Ϊnode
                fHead = newNode;
                //��Ϊ�ǿ���������ͷβһ��
                fTail = newNode;
            }
            else
            {
                //ԭ��ͷ�ڵ����һ��Ϊ�½ڵ�
                fHead.Prev = newNode;
                //�½ڵ����һ��Ϊԭ����ͷ�ڵ�
                newNode.Next = fHead;
                //��ͷ�ڵ�Ϊ�½ڵ�
                fHead = newNode;
            }
            //��С��һ
            fCount++;
        }

        public ListNode<T> AddFirst(T value)
        {
            ListNode<T> node = new ListNode<T>(this, value);
            AddFirst(node);
            return node;
        }
        /// <summary>
        /// ��ӽڵ㵽�����ĩβ
        /// </summary>
        /// <param name="value">Ҫ��ӵ�����</param>
        private void AddLast(ListNode<T> current)
        {
            if (fHead == null)
            {
                //��ͷ�ڵ�����Ϊnode
                fHead = current;
                //��Ϊ�ǿ���������ͷβһ��
                fTail = current;
            }
            else
            {
                //��ԭβ�ڵ����һ������Ϊ�½ڵ�
                fTail.Next = current;
                //���½ڵ����һ������Ϊԭβ�ڵ�
                current.Prev = fTail;
                //��β�ڵ���������Ϊ�½ڵ�
                fTail = current;
            }
            //��С��һ
            fCount++;
        }

        public ListNode<T> AddLast(T value)
        {
            ListNode<T> node = new ListNode<T>(this, value);
            AddLast(node);
            return node;
        }

        #endregion

        #region ɾ����������Ƴ��׽ڵ�β�ڵ�

        /// <summary>
        /// �Ƴ�ͷ�ڵ�
        /// </summary>
        private void RemoveFirst()
        {
            //���fCountΪ1���Ǿ����������
            if (fCount == 1)
                Clear();
            else
            {
                //��ͷ�ڵ���Ϊԭͷ������һ���ڵ㣬������һ���ڵ�����
                fHead = fHead.Next;
                //������һ���������⣬ԭ���ĵڶ����ڵ����һ����ͷ��㣬���ڵڶ���Ҫ���ͷ�ڵ㣬��Ҫ������Prev��Ϊnull���ܳ�Ϊͷ�ڵ�
                fHead.Prev = null;
                //��С��һ
                fCount--;
            }
        }

        /// <summary>
        /// �Ƴ�β�ڵ�
        /// </summary>
        private void RemoveLast()
        {
            //���fCountΪ1���Ǿ����������
            if (fCount == 1)
                Clear();
            else
            {
                //β�ڵ�����Ϊ�����ڶ����ڵ�
                fTail = fTail.Prev;
                //����β�ڵ��Next��Ϊnull����ʾ�����µ�β�ڵ�
                fTail.Next = null;
                //��С��һ
                fCount--;
            }
        }

        #endregion

        #region ��⵱ǰ�ڵ��Ƿ������������

        private void JudgeListContainCurrentException(ListNode<T> current)
        {
            TkDebug.AssertNotNull(fHead, "������Ϊ�գ�", this);
            //�жϵ�ǰ�ڵ��Ƿ�Ϊ��ǰ������
            //����LinkList<T>�����ListNode<T>���캯������Ϊ��ȷ����ͬ�Ľڵ��Ӧ�����Բ�ͬ����ڵ���������
            //�жϵ�ǰ�ڵ��Ƿ���this������
            TkDebug.Assert(current.List == this, "������ĵ�ǰ����ƥ�䣡", this);
        }

        #endregion

        #region ͨ��������ýڵ�

        /// <summary>
        /// ͨ��������ýڵ�
        /// </summary>
        /// <param name="index">����ֵ</param>
        /// <returns></returns>
        public ListNode<T> GetIndexNode(int index)
        {
            JudgeNormalException(index);
            ListNode<T> current;
            //���������ǰһ�룬��ô��ǰ�����
            if (index < (fCount >> 1))
            {
                current = fHead;
                for (int i = 0; i < index; ++i)
                    current = current.Next;
            }
            else//��������ں�һ�룬��ô�Ӻ���ǰ��
            {
                current = fTail;
                for (int i = fCount - 1; i > index; --i)
                    current = current.Prev;
            }
            return current;
        }

        #endregion

        /// <summary>
        /// ��������е�����
        /// </summary>
        public void Clear()
        {
            fHead = null;
            fTail = null;
            fCount = 0;
        }

        /// <summary>
        /// ����������ȡ�����еĽڵ�
        /// </summary>
        /// <param name="index">��������</param>
        /// <returns>�ڵ�</returns>
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
        /// �ڸ����Ľڵ㴦֮ǰ��������
        /// </summary>
        /// <param name="index">�ڵ�</param>
        /// <param name="value">Ҫ���������</param>
        public ListNode<T> AddBefore(ListNode<T> node, T value)
        {
            ListNode<T> newNode = new ListNode<T>(this, value);
            //���Ҫ��ӵ���ͷ�ڵ�
            if (node == fHead)
                AddFirst(newNode);
            else
            {
                //��ǰ�ڵ����һ������һ������Ϊ�½ڵ�
                node.Prev.Next = newNode;
                //�½ڵ����һ������Ϊ��ǰ�ڵ����һ��
                newNode.Prev = node.Prev;
                //�½ڵ����һ������Ϊ��ǰ�ڵ�
                newNode.Next = node;
                //��ǰ�ڵ����һ������Ϊ�½ڵ�
                node.Prev = newNode;
                //��С��һ
                fCount++;
            }
            return newNode;
        }

        /// <summary>
        /// �ڸ����Ľڵ㴦֮���������
        /// </summary>
        /// <param name="index">�ڵ�</param>
        /// <param name="value">Ҫ���������</param>
        public ListNode<T> AddAfter(ListNode<T> node, T value)
        {
            ListNode<T> newNode = new ListNode<T>(this, value);
            //���Ҫ��ӵ���β�ڵ�
            if (node == fTail)
                AddLast(newNode);
            else
            {
                //�½ڵ����һ���ڵ�����Ϊ��ǰ�ڵ�
                newNode.Prev = node;
                //�½ڵ����һ���ڵ�����Ϊ��ǰ�ڵ����һ���ڵ�
                newNode.Next = node.Next;
                //��ǰ�ڵ����һ���ڵ����һ���ڵ�����Ϊ�½ڵ�
                node.Next.Prev = newNode;
                //��ǰ�ڵ����һ���ڵ�����Ϊ�½ڵ�
                node.Next = newNode;
                //��С��һ
                fCount++;
            }
            return newNode;
        }

        /// <summary>
        /// �Ƴ������еĽڵ�
        /// </summary>
        /// <param name="index">Ҫ�Ƴ��Ľڵ�Ľڵ�</param>
        public void Remove(ListNode<T> node)
        {
            //���Ҫ�Ƴ�����ͷ�ڵ�
            if (node == fHead)
            {
                RemoveFirst();
                return;
            }
            //���Ҫ�Ƴ�����δ�ڵ�
            if (node == fTail)
            {
                RemoveLast();
                return;
            }
            //��ǰ�ڵ����һ����Next����Ϊ��ǰ�ڵ��Next
            node.Prev.Next = node.Next;
            //��ǰ�ڵ����һ����Prev����Ϊ��ǰ�ڵ��Prev
            node.Next.Prev = node.Prev;
            //��С��һ
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
        /// ͨ���ڵ㷵������ֵ
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
