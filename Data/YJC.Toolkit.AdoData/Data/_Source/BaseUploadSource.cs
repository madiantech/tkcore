using System;
using System.Data;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseUploadSource<T> : EmptyDbDataSource, ISource, IDisposable,
        ISupportMetaData where T : BaseUploadObject
    {
        private IMetaData fMetaData;

        protected BaseUploadSource()
        {
        }

        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            if (input.IsPost)
            {
                try
                {
                    T fileInfo = input.PostObject.Convert<T>();
                    if (string.IsNullOrEmpty(fileInfo.FileName))
                        throw new WebPostException("没有选择上传文件！");
                    return ProcessUploadFile(fileInfo);
                }
                catch (WebPostException ex)
                {
                    return OutputData.CreateToolkitObject(ex.CreateErrorResult());
                }
            }
            else
            {
                CreateDefaultValue(input, DataSet);
                input.CallerInfo.AddInfo(DataSet);

                return OutputData.Create(DataSet);
            }
        }

        #endregion ISource 成员

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Insert);
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData;
        }

        #endregion ISupportMetaData 成员

        public string TableName { get; set; }

        protected virtual void SetDefaultValue(IInputData input, ITableSchemeEx scheme,
            DataTable table, DataRow row)
        {
            foreach (DataColumn col in table.Columns)
            {
                string value = input.QueryString[col.ColumnName];
                if (!string.IsNullOrEmpty(value))
                    DataSetUtil.SetSafeValue(row, col, value);
            }

            var defaultFields = from field in scheme.Fields
                                where Tk5TableResolver.HasDefaultValue(field)
                                select field as ITk5FieldInfo;
            foreach (var field in defaultFields)
            {
                try
                {
                    row[field.NickName] = Expression.Execute(field.Edit.DefaultValue, DataSet, this);
                }
                catch
                {
                }
            }
        }

        protected virtual void CreateDefaultValue(IInputData input, DataSet dataSet)
        {
            if (string.IsNullOrEmpty(TableName) || fMetaData == null || input.Style.Style != PageStyle.Insert)
                return;

            ITableSchemeEx scheme = fMetaData.GetTableScheme(TableName);
            if (scheme == null)
                return;
            MetaDataTableResolver resolver = new MetaDataTableResolver(scheme, this);
            using (resolver)
            {
                //DataTable table = DataSetUtil.CreateDataTable(scheme.TableName, scheme.Fields);
                DataTable table = resolver.CreateVirtualTable(true);
                DataRow row = table.NewRow();
                table.Rows.Add(row);
                resolver.SetDefaultValue(input.QueryString);
                resolver.SetDefaultValue(row);
                resolver.FillCodeTable(input.Style);
                resolver.Decode(input.Style);
            }
        }

        protected abstract OutputData ProcessUploadFile(T fileInfo);
    }
}