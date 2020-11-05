using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class UploadExtension
    {
        public static Tk5UploadConfig AssertUpload(this IFieldInfoEx field)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            Tk5UploadConfig upload = field.Upload.Convert<Tk5UploadConfig>();
            TkDebug.AssertNotNull(upload, "非法调用，Field没有配置Upload单元", field);

            return upload;
        }

        public static IUploadProcessor CreateUploadProcessor(this Tk5UploadConfig config)
        {
            TkDebug.AssertArgumentNull(config, "config", null);

            if (config.Processor != null)
            {
                var creator = config.Processor as IConfigCreator<IUploadProcessor>;
                if (creator != null)
                    return creator.CreateObject();
            }
            return new DbUploadProcessor();
        }

        public static IUploadProcessor2 CreateUploadProcessor2(this Tk5UploadConfig config)
        {
            TkDebug.AssertArgumentNull(config, "config", null);

            if (config.Processor != null)
                return config.Processor.CreateObject();
            return new DbUploadProcessor();
        }
    }
}
