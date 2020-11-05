using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ActionResultData
    {
        private ActionResultData(ActionResult result, string message)
        {
            TkDebug.AssertArgumentNull(message, "message", null);

            Message = message;
            Result = result;
        }

        [SimpleAttribute]
        public string Message { get; private set; }

        [SimpleAttribute]
        public ActionResult Result { get; private set; }

        private static string GetTableName()
        {
            if (BaseAppSetting.Current == null)
                return "ActionResult";
            else
                return BaseAppSetting.Current.ActionResultName;
        }

        public void AddToDataSet(DataSet dataSet)
        {
            TkDebug.AssertArgumentNull(dataSet, "dataSet", this);


            DataTable table = EnumUtil.Convert(this).CreateTable(GetTableName());
            dataSet.Tables.Add(table);
        }

        public static ActionResultData CreateSuccessResult(string message)
        {
            return new ActionResultData(ActionResult.Success, message);
        }

        public static ActionResultData CreateReLogOnResult(string message)
        {
            return new ActionResultData(ActionResult.ReLogOn, message);
        }

        public static ActionResultData CreateErrorResult(string message)
        {
            return new ActionResultData(ActionResult.Error, message);
        }

        public static ActionResultData CreateFailResult(string message)
        {
            return new ActionResultData(ActionResult.Fail, message);
        }
    }
}
