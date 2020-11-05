using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [Serializable]
    public sealed class UserScript
    {
        private readonly List<ScriptConfig> fConfigs;

        /// <summary>
        /// Initializes a new instance of the UserScript class.
        /// </summary>
        /// <param name="configs"></param>
        public UserScript(List<ScriptConfig> configs)
        {
            fConfigs = configs;
        }

        public string CreateUserCss()
        {
            if (fConfigs == null)
                return string.Empty;

            var files = from config in fConfigs
                        where config.Type == ScriptType.Css && !string.IsNullOrEmpty(config.Content)
                        select string.Format(ObjectUtil.SysCulture,
                        "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\">", config.CreateContent());
            return string.Join(Environment.NewLine, files);
        }

        public string CreateUserJavaScript()
        {
            if (fConfigs == null)
                return string.Empty;

            var files = from config in fConfigs
                        where config.Type == ScriptType.JavaScript && !string.IsNullOrEmpty(config.Content)
                        select string.Format(ObjectUtil.SysCulture,
                        "<script type=\"text/javascript\" src=\"{0}\"></script>", config.CreateContent());
            return string.Join(Environment.NewLine, files);
        }
    }
}