using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IMediaService
    {
        //上传媒体文件
        [ApiMethod("/media/upload", Method = HttpMethod.Post)]
        MediaUploadResult UploadMedia([TkTypeConverter(typeof(LowerCaseEnumConverter), UseObjectType = true)]
            [ApiParameter]FileType type,
            [ApiParameter(Location = ParamLocation.File)]UploadData data);
    }
}