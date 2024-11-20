using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Pashamao.Service
{
    public class UserLogin
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

            if (user == null)
            {
                return (false);
            }

            //取得鹽值
            string salt = user.Hash.Substring ( 0, 24 );

            //用鹽值 hashPwd 之後, 比對

            return (HashPwd ( pwd, salt ) == user.Hash);
        }

        /// <summary>
        /// 後踢前
        /// </summary>
        public void KickOutPrevious()
        {

        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="pwd">想加密的密碼</param>
        /// <param name="salt">加密用鹽值 可以生成也可取舊的</param>
        /// <returns></returns>
        public string HashPwd( string pwd, string salt )
        {
            try
            {

                byte[] saltBytes = Convert.FromBase64String ( salt );
                byte[] pwdBytes = Encoding.UTF8.GetBytes ( pwd );
                byte[] combinedPwdBytes = new byte[saltBytes.Length + pwdBytes.Length];

                saltBytes.CopyTo ( combinedPwdBytes, 0 );

                pwdBytes.CopyTo ( combinedPwdBytes, saltBytes.Length );

                using (SHA256 sha256 = SHA256.Create ())
                {

                    byte[] inputHashBytes = sha256.ComputeHash ( combinedPwdBytes );
                    string inputHashStr = salt + Convert.ToBase64String ( inputHashBytes );

                    //轉成16進制後回傳 資料庫已鹽值+加密後pwd儲存
                    return inputHashStr;

                }
            } catch (Exception e)
            {
                logger.Error ( e );
                throw e;
            }

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

                    // 將字節數組轉換為可讀的十六進制字符串 回傳(因為1個字節組數代表 1個16進位(xx)=>有32字元)
                    return Convert.ToBase64String ( saltBytes );
                }
            } catch (Exception e)
            {
                logger.Error ( e );
                throw e;
            }
            //16個字節組數


        }


    }
}