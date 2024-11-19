using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Pashamao.ServerCs
{
    public class PwdHash
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();

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

                string conbinedPwdStr = pwd + salt;
                byte[] combinedPwdBytes = System.Text.Encoding.UTF8.GetBytes ( conbinedPwdStr );

                using (SHA256 sha256 = SHA256.Create ())
                {

                    byte[] inputHashBytes = sha256.ComputeHash ( combinedPwdBytes );

                    //轉成16進制後回傳 資料庫已鹽值+加密後pwd儲存
                    return (salt + ConvertHexadecimal ( inputHashBytes ));

                }
            } catch (Exception e)
            {
                logger.Error ( e );
                throw e;
            }

        }


        /// <summary>
        /// 驗證密碼是否正確
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool VarifyPwd( string pwd )
        {
            //找到資料庫pwd_hash

            //取得鹽值

            //用鹽值 hashPwd 之後, 比對


            return false;
        }



        /// <summary>
        /// 取得雖機生成salt
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
                    return ConvertHexadecimal ( saltBytes );
                }
            } catch (Exception e)
            {
                logger.Error ( e );
                throw e;
            }
            //16個字節組數


        }

        /// <summary>
        /// 從資料庫字串取鹽
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public string GetDbSalt( string hash )
        {
            try
            {
                string salt = hash.Substring ( 0, 31 );
                return salt;
            } catch (Exception e)
            {
                logger.Error ( e );
                throw e;
            }
        }


        /// <summary>
        /// 將字節組數轉為16進制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string ConvertHexadecimal( byte[] bytes )
        {

            try
            {
                // 將字節數組轉換為可讀的十六進制字符串
                StringBuilder stringBuilder = new StringBuilder ();

                foreach (byte b in bytes)
                {
                    stringBuilder.AppendFormat ( "{0:x2}", b );
                }
                //{0x12, 0xa}會變成 120a

                return stringBuilder.ToString ();
            } catch (Exception e)
            {
                logger.Error ( e );
                throw e;
            }


        }

    }
}