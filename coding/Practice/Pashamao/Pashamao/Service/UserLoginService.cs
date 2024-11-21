using Microsoft.Ajax.Utilities;
using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using Pashamao.Utility;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;

namespace Pashamao.Service
{
    public class UserLoginService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();

        /// <summary>
        /// 驗證密碼是否正確
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool VarifyUser( string acct, string pwd )
        {
            //找到資料庫pwd_hash
            UserRepository userRepository = new UserRepository ();

            User user = userRepository.sqlUserPwd ( acct );
            
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

            //如果有帳號密碼存在且匹配 把資料庫舊session id統一清理掉, 放入新的

            correctUser = (hashUtility.HashPwd(pwd, salt) == user.Hash);
            if (correctUser)
            {
                user.SessionId = HttpContext.Current.Session.SessionID;
                userRepository.sqlEditSession (user);
            }

            //用鹽值 hashPwd 之後, 比對
            return correctUser;
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