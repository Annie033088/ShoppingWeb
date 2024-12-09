﻿using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Filters
{
    public class UserKickOutFilter : ActionFilterAttribute
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (HttpContext.Current.Session["UserVisitState"] == null)
                {
                    HttpContext.Current.Session["UserVisitState"] = "Guest";
                    filterContext.Result = new RedirectResult("/Login/Index");
                    base.OnActionExecuting(filterContext);
                    return;
                }

                if (HttpContext.Current.Session["UserVisitState"].ToString() == "Guest")
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                    base.OnActionExecuting(filterContext);
                    return;
                }

                UserSessionModel userModel = HttpContext.Current.Session["UserSession"] as UserSessionModel;

                if (userModel == null)
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
                else
                {
                    //取得DB的sessionId
                    UserRepository userRepository = new UserRepository();
                    bool DBStatus;
                    string DBSessionId;
                    (DBStatus, DBSessionId) = userRepository.GetStatusAndSessionId(userModel);

                    if (DBSessionId == null)
                    {
                        filterContext.Result = new RedirectResult("/Login/Index");
                    }

                    if (DBStatus == false)
                    {
                        filterContext.Controller.TempData["KickOutMessage"] = "您的帳號已被禁止使用";
                        filterContext.Result = new RedirectResult("/Login/Index");
                    }

                    //判斷現sessionId與資料庫的sessionId是否相同, 不同的話清除session並且重定向到Login
                    if (DBSessionId != HttpContext.Current.Session.SessionID)
                    {
                        filterContext.Controller.TempData["KickOutMessage"] = "您的帳號已被他人踢出";
                        filterContext.Result = new RedirectResult("/Login/Index");
                    }
                }
                base.OnActionExecuting(filterContext);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }

        }
    }
}