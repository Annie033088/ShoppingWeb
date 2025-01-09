namespace Pashamao.Models
{
    public enum ErrorCodeDefine
    {
        /// <summary>
        /// 預設
        /// </summary>
        Default = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 被他人踢出
        /// </summary>
        KickOut = 2,

        /// <summary>
        /// 被Ban掉
        /// </summary>
        Baned = 3,

        /// <summary>
        /// 權限已被修改
        /// </summary>
        PermissionModified = 4,

        /// <summary>
        /// 無效輸入
        /// </summary>
        InvalidFormatOrEntry = 5,

        /// <summary>
        /// 伺服器錯誤
        /// </summary>
        ServerError = 6,

        /// <summary>
        /// 無權限
        /// </summary>
        NoPermission = 7,

        /// <summary>
        /// 使用者未登入
        /// </summary>
        UserNotLogged = 8,

        /// <summary>
        /// 登入失敗
        /// </summary>
        LoginFailed = 9,

        /// <summary>
        /// 密碼輸入錯誤
        /// </summary>
        PasswordEnterIncorrectly = 10,

        /// <summary>
        /// 創建失敗
        /// </summary>
        CreateFailed = 11,

        /// <summary>
        /// 修改失敗
        /// </summary>
        ModifiedFailed = 12,

        /// <summary>
        /// 刪除失敗
        /// </summary>
        DeleteFailed = 13
    }
}