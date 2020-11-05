using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    /// <summary>
    /// 不适合Odbc，OleDb这样的通用数据连接
    /// </summary>
    public class AutoStoredProc : AbstractStoredProc
    {
        private readonly List<ParameterInfo> fInputProperties = new List<ParameterInfo>();
        private readonly List<ParameterInfo> fOutputProperties = new List<ParameterInfo>();

        public AutoStoredProc(string procName)
            : base(procName)
        {
        }

        public AutoStoredProc(string procName, IDbConnection connection)
            : base(procName, connection)
        {
        }

        public AutoStoredProc(string procName, TkDbContext context)
            : base(procName, context)
        {
        }

        protected sealed override void PrepareParameters()
        {
            PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = from memberInfo in properties
                             from attribute in memberInfo.GetCustomAttributes(false)
                             where attribute is ParameterAttribute
                             select new { Property = memberInfo, Attribute = (ParameterAttribute)attribute };
            foreach (var item in parameters)
            {
                ParameterAttribute attribute = item.Attribute;
                string name;
                if (string.IsNullOrEmpty(attribute.Name))
                {
                    name = item.Property.Name;
                    switch (attribute.LetterCase)
                    {
                        case CaseCategory.Uppercase:
                            name = name.ToUpper(ObjectUtil.SysCulture);
                            break;
                        case CaseCategory.Lowercase:
                            name = name.ToLower(ObjectUtil.SysCulture);
                            break;
                    }
                }
                else
                    name = attribute.Name;
                TkDataType dataType = attribute.UseDefaultType ?
                    MetaDataUtil.ConvertTypeToDataType(item.Property.PropertyType) :
                    attribute.DataType;
                StoredProcParameter param = Add(name, attribute.Direction, dataType);
                if (attribute.Size > 0)
                    param.Size = attribute.Size;
                else
                {
                    if (dataType == TkDataType.String)
                        param.Size = 255;
                }
                ParameterInfo info = new ParameterInfo { Attribute = attribute, Parameter = param, Property = item.Property };
                if (DbUtil.IsInputParameter(attribute.Direction))
                    fInputProperties.Add(info);
                if (DbUtil.IsOutputParameter(attribute.Direction))
                    fOutputProperties.Add(info);

                IDbDataParameter parameter = CreateDataParameter(param);
                info.SetInputValue(this);
                Command.Parameters.Add(parameter);
            }
        }

        protected sealed override void SetInputValues()
        {
            foreach (ParameterInfo param in fInputProperties)
                param.SetInputValue(this);
        }

        protected sealed override void SetOutputValues()
        {
            foreach (ParameterInfo param in fOutputProperties)
                param.SetOutputValue(this);
        }
    }
}
