using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using Pashamao.Utility;
using System;
using System.Security.Cryptography;
using System.Web;

namespace Pashamao.Service
{
    public class UserLoginService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();


        private UserRepository userRepository;
        private User user;

        public UserLoginService( string loginAcct )
        {
            //取得使用者資料
            userRepository = new UserRepository ();
            user = userRepository.LoginGetUser ( loginAcct );
        }

        /// <summary>
        /// 驗證密碼是否正確
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        internal bool VarifyUserPwd( string loginPwd )
        {

            //宣告加密工具的變數
            HashUtility hashUtility = new HashUtility ();

            //帳號匹配成功與否
            bool correctUser;

            if (user == null)
            {
                return (false);
            }

            //取得鹽值
            string salt = user.Hash.Substring ( 0, 24 );

            //用鹽值 hashPwd 之後, 比對
            correctUser = (hashUtility.HashPwd ( loginPwd, salt ) == user.Hash);
            return correctUser;
        }

        /// <summary>
        /// 是不是禁用帳號
        /// </summary>
        /// <returns></returns>
        internal bool AcctSuspended()
        {
            return !(user.Status);
        }

        /// <summary>
        /// 設定session資料
        /// </summary>
        internal void SetUserSession()
        {
            //把資料庫舊session id統一清理掉, 放入新的
            user.SessionId = HttpContext.Current.Session.SessionID;
            userRepository.EditSessionId ( user );

            //把需要的使用者資料放入session
            SessionModel userSession = new SessionModel ();
            userSession.UID = user.UID;
            userSession.Account = user.Account;
            userSession.Name = user.Name;
            userSession.RoleId = user.RoleId;
            HttpContext.Current.Session["UserSession"] = userSession;

        }


        /// <summary>
        /// 雖機生成salt(可以放在新增用戶)
        /// </summary>
        /// <returns></returns>
        public string GenerateSalt()
        {
            try
            {
                int len = 16;

                using (RandomNumberGenerator rng = RandomNumberGenerator.Create ())
                {
                    byte[] saltBytes = new byte[len];
                    rng.GetBytes ( saltBytes ); // 填充隨機數據

                    // 將字節數組轉換為可讀的字符串 回傳
                    return Convert.ToBase64String ( saltBytes );
                }
            } catch (Exception e)
            {
                logger.Error ( e );
                throw e;
            }
        }


    }
}