using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Data.Constraint;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class EasySearchConstraint : BaseConstraint
    {
        private readonly string fRegName;

        public EasySearchConstraint(IFieldInfo field, string regName)
            : base(field)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", null);

            IsFirstCheck = true;
            HdFieldName = "hd" + field.NickName;
            fRegName = regName;
        }

        protected override FieldErrorInfo CheckError(IInputData inputData,
            string value, int position, params object[] args)
        {
            DataRow row = GetPostRow(inputData, position);
            string hdFieldValue = row[HdFieldName].ToString();

            if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(hdFieldValue))
                return null;

            EasySearch easySearch = PlugInFactoryManager.CreateInstance<EasySearch>(
                EasySearchPlugInFactory.REG_NAME, fRegName);
            string newValue = value;
            EasySearchErrorType error = easySearch.CheckName(hdFieldValue, ref newValue, args);
            if (error == EasySearchErrorType.None)
            {
                if (newValue != value)
                {
                    if (string.IsNullOrEmpty(newValue))
                        row[Field.NickName] = DBNull.Value;
                    else
                        row[Field.NickName] = newValue;
                }
                return null;
            }
            return CreateErrorObject(error);
        }


        public string EasySearchName { get; private set; }

        public string HdFieldName { get; private set; }

        protected FieldErrorInfo CreateErrorObject(EasySearchErrorType error)
        {
            string errorMsg = string.Empty;
            switch (error)
            {
                case EasySearchErrorType.NotExist:
                    errorMsg = Field.DisplayName + TkWebApp.EasySearchNotExist;
                    break;
                case EasySearchErrorType.VariousTwo:
                    errorMsg = Field.DisplayName + TkWebApp.EasySearchVariousTwo;
                    break;
            }

            return CreateErrorObject(errorMsg);
        }

        protected DataRow GetPostRow(IInputData inputData, int position)
        {
            DataSet dataSet = inputData.PostObject.Convert<DataSet>();
            DataTable postTable = dataSet.Tables[TableName];
            TkDebug.Assert(postTable.Columns.Contains(HdFieldName), string.Format(ObjectUtil.SysCulture,
                "提交上的表{0}中没有包含字段{1}，请检查提交的Xml", TableName, HdFieldName), this);

            DataRow row = postTable.Rows[position];
            return row;
        }
    }
}
