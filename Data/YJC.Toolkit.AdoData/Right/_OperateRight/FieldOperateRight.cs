using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class FieldOperateRight : IOperateRight
    {
        private FieldOperateRightItem fNullItem;
        private readonly RegNameList<FieldOperateRightItem> fList;

        public FieldOperateRight(string fieldName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fieldName, "fieldName", null);

            fList = new RegNameList<FieldOperateRightItem>();
            FieldName = fieldName;
        }

        #region IOperateRight 成员

        public virtual IEnumerable<string> GetOperator(OperateRightEventArgs e)
        {
            DataRow row = e.Row;
            if (row == null)
                return EmptyRowRights;
            try
            {
                object value = row[FieldName];
                if (value == DBNull.Value)
                {
                    if (fNullItem != null)
                        return fNullItem.Rights;
                }
                else
                {
                    FieldOperateRightItem item = fList[value.ToString()];
                    if (item != null)
                        return item.Rights;
                }
            }
            catch
            {
            }
            return null;
        }

        #endregion

        public string FieldName { get; private set; }

        public IEnumerable<string> EmptyRowRights { get; set; }

        public void AddItem(FieldOperateRightItem item)
        {
            TkDebug.AssertArgumentNull(item, "item", this);

            fList.Add(item);
            if (fNullItem == null && item.ContainsNull)
                fNullItem = item;
        }
    }
}
