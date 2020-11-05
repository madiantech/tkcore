namespace YJC.Toolkit.Right
{
    public interface IPasswordProvider
    {
        /// <summary>
        /// 获取有效密码中必须包含的最少特殊字符数。
        /// </summary>
        int MinRequiredNonAlphanumericCharacters { get; }

        /// <summary>
        /// 获取密码所要求的最小长度。
        /// </summary>
        int MinRequiredPasswordLength { get; }

        /// <summary>
        /// 验证密码的健壮性
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>如果符合健壮性，则大于0，健壮性越强，值越大；否则为0</returns>
        int ValidateStrength(string password);

        /// <summary>
        /// 在成员资格数据存储区中存储前对密码进行加密等处理
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>在成员资格数据存储区中存储的指定格式的密码</returns>
        string Format(string password);
    }
}