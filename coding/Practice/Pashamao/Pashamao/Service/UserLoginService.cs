﻿using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using Pashamao.Utility;
using System;
using System.Web;
using System.Web.Mvc;

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
            user = userRepository.VerifyAndGetUser(loginAcct, loginPwd, HttpContext.Current.Session.SessionID);

            //帳號匹配成功與否
            if (user == null)
            {
                return (false);
            }
            else
            {
                SessionModel userSession = new SessionModel();
                userSession.UID = user.UID;
                userSession.RoleId = user.RoleId;
                HttpContext.Current.Session["userSession"] = userSession;
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