using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System.Web;

namespace Pashamao.Service
{
    public class UserLoginService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private UserRepository userRepository;
        private User user;

        public UserLoginService()
        {
            //取得使用者資料
            userRepository = new UserRepository();
            user = new User();
        }

        /// <summary>
        /// 驗證密碼是否正確,並取得user
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        internal bool VerifyAndGetUser(string loginAcct, string loginPwd)
        {
            long userPermission;
            (user, userPermission) = userRepository.VerifyAndGetUser(loginAcct, loginPwd, HttpContext.Current.Session.SessionID);

            //帳號匹配成功與否
            if (user == null)
            {
                return (false);
            }
            else
            {
                UserSessionModel userSession = new UserSessionModel();
                userSession.UID = user.UID;
                userSession.UserPermission = userPermission;
                HttpContext.Current.Session["UserSession"] = userSession;
                return (true);
            }
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
        /// 取得user的暱稱或帳號
        /// </summary>
        /// <returns></returns>
        internal string GetUserName()
        {
            return user.Name == "null" ? user.Account : user.Name;
        }

    }
}