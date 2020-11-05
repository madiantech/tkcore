using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class ModelUtil
    {
        public static string GetEditTitle(IEditModel model, BootcssEditData editData, string title)
        {
            TkDebug.AssertArgumentNull(model, "model", null);
            TkDebug.AssertArgumentNull(editData, "editData", null);

            switch (model.PageStyle)
            {
                case "Insert":
                    return string.Format(ObjectUtil.SysCulture, editData.InsertFormat, title);

                case "Update":
                    return string.Format(ObjectUtil.SysCulture, editData.EditFormat, title);

                default:
                    return string.Empty;
            }
        }

        public static string GetPageId(string pageStyle)
        {
            return string.Format(ObjectUtil.SysCulture, "Web{0}XmlPage", pageStyle);
        }
    }
}