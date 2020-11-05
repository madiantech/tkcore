using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin;

namespace YJC.Toolkit.WeCorp.Service
{
    public interface IMediaService
    {
        //上传临时素材
        [ApiMethod("/media/upload", Method = HttpMethod.Post)]
        MediaUploadResult UploadMedia(
            [TkTypeConverter(typeof(LowerCaseEnumConverter), UseObjectType = true), ApiParameter]FileType type,
            [ApiParameter(Location = ParamLocation.File)]UploadData data);

        //上传图片
        [ApiMethod("/media/uploadimg", Method = HttpMethod.Post, ResultKey = "url")]
        string UploadImage([ApiParameter(Location = ParamLocation.File)]UploadData data);

        //获取临时素材
        [ApiMethod("/media/get", DownloadFile = true)]
        DownloadData DownloadMedia([ApiParameter(NamingRule = NamingRule.UnderLineLower)]string mediaId);

        //获取高清语言素材
        [ApiMethod("/media/get/jssdk", DownloadFile = true)]
        DownloadData DownloadJssdkMedia([ApiParameter(NamingRule = NamingRule.UnderLineLower)]string mediaId);
    }
}