using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SwitchConfig : IReadObjectCallBack
    {
        internal SwitchConfig()
        {
        }

        public SwitchConfig(string nickName)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", null);

            NickName = nickName;
            OpenValue = "1";
            CloseValue = "0";
        }

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (InternalErrorMessage != null)
                ErrorMessage = InternalErrorMessage.ToString();
        }

        #endregion IReadObjectCallBack 成员

        [SimpleAttribute]
        public string NickName { get; private set; }

        [SimpleAttribute(DefaultValue = "1")]
        public string OpenValue { get; set; }

        [SimpleAttribute(DefaultValue = "0")]
        public string CloseValue { get; set; }

        [SimpleAttribute]
        public bool UpdateTrackField { get; set; }

        [SimpleAttribute]
        public SwitchNullProcessMethod NullValueProcess { get; set; }

        [ObjectElement(NamespaceType.Toolkit, LocalName = "ErrorMessage")]
        internal MultiLanguageText InternalErrorMessage { get; private set; }

        public string ErrorMessage { get; set; }

        public void Switch(TableResolver resolver, DataRow row)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(row, "row", null);

            object value = row[NickName];
            row.BeginEdit();
            try
            {
                if (value == DBNull.Value)
                {
                    switch (NullValueProcess)
                    {
                        case SwitchNullProcessMethod.OpenValue:
                            row[NickName] = OpenValue;
                            break;

                        case SwitchNullProcessMethod.ThrowException:
                            throw new WebPostException(ErrorMessage);
                    }
                }
                else if (value.ToString() == OpenValue)
                    row[NickName] = CloseValue;
                else
                    row[NickName] = OpenValue;
                if (UpdateTrackField)
                    resolver.UpdateTrackField(UpdateKind.Update, row);
            }
            finally
            {
                row.EndEdit();
            }
            resolver.SetCommands(AdapterCommand.Update);
            resolver.UpdateDatabase();
        }
    }
}