using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public sealed class FieldErrorInfoCollection : DictionaryList<FieldErrorInfo>
    {
        internal void Add(BaseConstraint constraint, FieldErrorInfo errorObject, int i)
        {
            string key = CreateKey(constraint, i);
            Add(key, errorObject);
        }

        private static string CreateKey(BaseConstraint constraint, int i)
        {
            string key = string.Format(ObjectUtil.SysCulture, "{0}.{1}-{2}",
                constraint.TableName, constraint.Field.NickName, i);
            return key;
        }

        private static string CreateKey(FieldErrorInfo errorObject)
        {
            string key = string.Format(ObjectUtil.SysCulture, "{0}.{1}-{2}",
                errorObject.TableName, errorObject.NickName, errorObject.Position);
            return key;
        }

        internal bool Contains(BaseConstraint constraint, int i)
        {
            string key = CreateKey(constraint, i);
            return ContainsKey(key);
        }

        public void Add(FieldErrorInfo errorObject)
        {
            string key = CreateKey(errorObject);
            Add(key, errorObject);
        }

        public void CheckError()
        {
            if (Count > 0)
                throw new WebPostException(TkWebApp.ResourceManager.GetString(
                    "ConstraintError", TkWebApp.Culture), this);
        }
    }
}
