using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Pashamao.Utility
{
    public class HashUtility
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();
        /// <summary>
        /// 加密pwd
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

                    //資料庫以鹽值+加密後pwd儲存
                    return inputHashStr;
                }
            } catch (Exception e)
            {
                logger.Error ( e );
                throw e;
            }

        }
    }
}