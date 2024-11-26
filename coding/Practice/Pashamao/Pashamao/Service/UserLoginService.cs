using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using Pashamao.Utility;
using System.Web;

namespace Pashamao.Service
{
    public class UserLoginService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();


        private UserRepository userRepository;
        private User user;

        public UserLoginService( )
        {
            //取得使用者資料
            userRepository = new UserRepository ();
            user = new User ();
        }

        /// <summary>
        /// 驗證密碼是否正確
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        internal bool VarifyUser( string loginAcct, string loginPwd)
        {
            //宣告加密工具的變數
            HashUtility hashUtility = new HashUtility();

            user = userRepository.VerifyAndGetUser( loginAcct, loginPwd, HttpContext.Current.Session.SessionID );

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



    }
}