using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public interface IFunctionRight
    {
        /// <summary>
        /// 获得html菜单的脚本
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>返回该用户所能访问的菜单的html脚本</returns>
        DataSet GetMenuObject(object userId);

        /// <summary>
        /// 获得是否可以使用该功能
        /// </summary>
        /// <param name="key">功能Id</param>
        /// <returns>如果可以使用，返回true；否则返回false</returns>
        bool IsFunction(object key);

        /// <summary>
        /// 获得是否可以使用功能下的子功能
        /// </summary>
        /// <param name="subKey">子功能Id</param>
        /// <param name="key">功能Id</param>
        /// <returns>如果可以使用，返回true；否则返回false</returns>
        bool IsSubFunction(SubFunctionKey subKey, object key);

        /// <summary>
        /// 是否为管理员
        /// </summary>
        /// <returns>是/否</returns>
        bool IsAdmin();

        //IEnumerable<string> GetSubFunctions(object key);

        /// <summary>
        /// 初始化功能权限
        /// </summary>
        /// <param name="data">用户自定义数据</param>
        void Initialize(IUserInfo user);
    }
}