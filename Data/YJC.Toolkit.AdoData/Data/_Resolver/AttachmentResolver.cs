using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class AttachmentResolver : TableResolver
    {
        public AttachmentResolver(IDbDataSource source)
            : base(MetaDataUtil.CreateTableScheme("Attachment.xml"), source)
        {
        }

        public int Insert(string name, string path, string contentType, byte[] data, bool commitDb)
        {
            int id = -1;
            DataRow row = NewRow();
            row.BeginEdit();
            try
            {
                id = Context.GetUniId(TableName).Value<int>();
                row["Id"] = id;
                row["OriginalName"] = name;
                row["ServerPath"] = path;
                row["ContentType"] = contentType;
                row["Size"] = data == null ? 0 : data.Length;
                row["Content"] = data;
            }
            finally
            {
                row.EndEdit();
            }
            if (commitDb)
            {
                SetCommands(AdapterCommand.Insert);
                UpdateDatabase();
            }
            return id;
        }

        public void Delete(int id, bool commitDb)
        {
            DataRow row = TrySelectRowWithKeys(id);
            if (row == null)
                return;
            row.Delete();
            if (commitDb)
            {
                SetCommands(AdapterCommand.Delete);
                UpdateDatabase();
            }
        }

        public void Update(int id, byte[] data, bool commitDb)
        {
            DataRow row = TrySelectRowWithKeys(id);
            if (row == null)
                return;
            row.BeginEdit();
            try
            {
                row["Size"] = data == null ? 0 : data.Length;
                row["Content"] = data;
            }
            finally
            {
                row.EndEdit();
            }
            if (commitDb)
            {
                SetCommands(AdapterCommand.Update);
                UpdateDatabase();
            }
        }

        public void Update(int id, string name, string path, string contentType, byte[] data, bool commitDb)
        {
            DataRow row = TrySelectRowWithKeys(id);
            if (row == null)
                return;
            row.BeginEdit();
            try
            {
                row["OriginalName"] = name;
                row["ServerPath"] = path;
                row["ContentType"] = contentType;
                row["Size"] = data == null ? 0 : data.Length;
                row["Content"] = data;
            }
            finally
            {
                row.EndEdit();
            }
            if (commitDb)
            {
                SetCommands(AdapterCommand.Update);
                UpdateDatabase();
            }
        }
    }
}