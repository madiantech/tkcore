using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    internal class MethodDataInfo
    {
        private static readonly Type DictionaryType = typeof(Dictionary<,>);
        private static readonly Type StringType = typeof(string);

        private readonly ApiMethodAttribute fAttribute;

        private readonly List<ParamDataInfo> fUrlParams;
        private readonly List<ParamDataInfo> fPartialParams;
        private readonly ParamDataInfo fContentParam;
        private readonly ParamDataInfo fFileParam;
        private readonly ITkTypeConverter fConverter;
        private readonly bool fIsMultiple;
        private readonly bool fIsSimpleType;
        private readonly Type fObjectType;
        private readonly Type fCollectionType;

        public MethodDataInfo(MethodInfo method, ApiMethodAttribute attribute)
        {
            MethodName = method.Name;
            fAttribute = attribute;
            var returnType = method.ReturnType;
            fIsMultiple = attribute.IsMultiple;
            if (fIsMultiple)
            {
                fCollectionType = attribute.CollectionType ?? returnType;
                if (attribute.ObjectType != null)
                    fObjectType = attribute.ObjectType;
                else
                {
                    TkDebug.Assert(returnType.IsGenericType, string.Format(ObjectUtil.SysCulture,
                        "无法从类型{0}获取ObjectType，因为其不是泛型类型", returnType), this);
                    Type[] args = returnType.GetGenericArguments();
                    TkDebug.Assert(args.Length == 1, string.Format(ObjectUtil.SysCulture,
                       "类型{0}的泛型参数个数不为1，无法获取第一个泛型参数", returnType), this);
                    fObjectType = args[0];
                }
            }
            else
            {
                fObjectType = attribute.ObjectType ?? returnType;
            }
            switch (attribute.ResultType)
            {
                case ResultType.Auto:
                    if (fIsMultiple)
                        fIsSimpleType = false;
                    else
                    {
                        fConverter = TkTypeDescriptor.GetSimpleConverter(fObjectType);
                        fIsSimpleType = fConverter != null;
                    }
                    break;

                case ResultType.Simple:
                    fIsSimpleType = true;
                    break;

                case ResultType.Object:
                    fIsSimpleType = false;
                    break;
            }
            fUrlParams = new List<ParamDataInfo>();
            fPartialParams = new List<ParamDataInfo>();
            foreach (var param in method.GetParameters())
            {
                ApiParameterAttribute paramAttr = Attribute.GetCustomAttribute(param,
                    typeof(ApiParameterAttribute)) as ApiParameterAttribute;
                if (paramAttr != null)
                {
                    ParamDataInfo paramInfo = new ParamDataInfo(param, paramAttr);
                    switch (paramInfo.Location)
                    {
                        case ParamLocation.Url:
                            fUrlParams.Add(paramInfo);
                            break;

                        case ParamLocation.Partial:
                            fPartialParams.Add(paramInfo);
                            break;

                        case ParamLocation.Content:
                        case ParamLocation.ContentDictionary:
                            fContentParam = paramInfo;
                            break;

                        case ParamLocation.File:
                            fFileParam = paramInfo;
                            break;
                    }
                }
            }
        }

        public string MethodName { get; private set; }

        private Uri GetUrl(IIMPlatform platform, object[] args)
        {
            UrlBuilder builder = new UrlBuilder(platform.BaseUri)
            {
                Path = fAttribute.UriPath
            };
            if (fAttribute.UseAccessToken)
            {
                string token = platform.AccessToken;
                builder.AddQueryString(platform.QueryStringName, token);
            }
            foreach (var param in fUrlParams)
            {
                object paramValue = args[param.Index];
                string value;
                if (param.Converter != null)
                    value = param.Converter.ConvertToString(paramValue, ObjectUtil.WriteSettings);
                else
                    value = ObjectUtil.ToString(paramValue, ObjectUtil.WriteSettings);
                builder.AddQueryString(param.ParamName, value);
            }
            return builder.ToUri();
        }

        public object Execute(IIMPlatform platform, object[] args)
        {
            Uri url = GetUrl(platform, args);
            TkTrace.LogInfo($"IM Url: {url}");
            BaseResult result = CreateResult();

            switch (fAttribute.Method)
            {
                case HttpMethod.Get:
                    IMUtil.GetFromUri(url, fAttribute.ResultModelName, result, fAttribute.DownloadFile);
                    break;

                case HttpMethod.Post:
                    if (fContentParam != null)
                    {
                        object contentData = args[fContentParam.Index];
                        string json;
                        if (contentData == null)
                            json = string.Empty;
                        else
                        {
                            if (fContentParam.Location == ParamLocation.Content)
                                json = contentData.WriteJson(fContentParam.ModelName);
                            else
                            {
                                Type dictType = DictionaryType.MakeGenericType(StringType, contentData.GetType());
                                IDictionary dict = ObjectUtil.CreateObject(dictType).Convert<IDictionary>();
                                dict.Add(fContentParam.DictionaryName, contentData);
                                json = dict.WriteJson(fContentParam.ModelName);
                            }
                        }

                        IMUtil.PostToUri(url, fAttribute.ResultModelName, json, result);
                    }
                    else if (fFileParam != null)
                    {
                        object fileData = args[fFileParam.Index];
                        TkDebug.Assert(fileData is UploadData, string.Format(ObjectUtil.SysCulture,
                            "第{0}个参数必须是UploadData类型的，但是现在是{1}类型", fFileParam.Index,
                            fileData != null ? fileData.GetType().ToString() : string.Empty), fAttribute.Method);
                        UploadData upload = (UploadData)fileData;
                        IMUtil.UploadFile(url, upload.FileName, upload.FileData, result);
                    }
                    else if (fPartialParams.Count > 0)
                    {
                        TkDebug.AssertNotNull(fAttribute.PostType, string.Format(ObjectUtil.SysCulture,
                            "方法{0}定义了Partial类型的参数，但是没有在ApiMethod中声明PostType属性", MethodName), this);
                        object[] values = new object[fPartialParams.Count];
                        for (int i = 0; i < fPartialParams.Count; ++i)
                            values[i] = args[fPartialParams[i].Index];
                        object contentData = ObjectUtil.CreateObject(fAttribute.PostType, values);
                        string json;
                        if (contentData != null)
                            json = contentData.WriteJson(fAttribute.PostModelName);
                        else
                            json = string.Empty;
                        IMUtil.PostToUri(url, fAttribute.ResultModelName, json, result);
                    }
                    break;
            }
            Object returnResult = string.IsNullOrEmpty(fAttribute.ResultKey) ? result
                : ((CustomObject)result).Data;
            return returnResult;
        }

        private BaseResult CreateResult()
        {
            BaseResult result;
            if (fAttribute.DownloadFile)
                return new DownloadData();

            string resultKey = fAttribute.ResultKey;
            if (string.IsNullOrEmpty(resultKey))
            {
                result = (fAttribute.UseConstructor ? ObjectUtil.CreateObjectWithCtor(fObjectType)
                    : ObjectUtil.CreateObject(fObjectType)).Convert<BaseResult>();
            }
            else
            {
                if (fIsMultiple)
                {
                    if (fIsSimpleType)
                        result = new CustomObject(resultKey, fObjectType,
                            fConverter, fCollectionType);
                    else
                        result = new CustomObject(resultKey, fObjectType,
                            fAttribute.UseConstructor, fCollectionType);
                }
                else
                {
                    if (fIsSimpleType)
                        result = new CustomObject(resultKey, fObjectType, fConverter);
                    else
                        result = new CustomObject(resultKey, fObjectType, fAttribute.UseConstructor);
                }
            }
            return result;
        }

        public override string ToString() => $"函数{MethodName}";
    }
}