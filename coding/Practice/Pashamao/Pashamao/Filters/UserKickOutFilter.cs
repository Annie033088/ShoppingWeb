using NLog;
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
        private UserRepository userRepository = new UserRepository();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                UserSessionModel userSessionModel = HttpContext.Current.Session["UserSession"] as UserSessionModel;

                if (userSessionModel == null)
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
                else
                {
                    //取得DB的sessionId
                    bool UserStatus;
                    string UserSessionId;
                    long UserPermissions;
                    (UserStatus, UserSessionId, UserPermissions) = userRepository.GetUserStatusSessionIdPermissions(userSessionModel.UserId);

                    if (UserStatus == false)
                    {
                        filterContext.Controller.TempData["KickOutMessage"] = "您的帳號已被禁止使用";
                        filterContext.Result = new RedirectResult("/Login/Index");
                        base.OnActionExecuting(filterContext);
                        return;
                    }

                    //判斷現sessionId與資料庫的sessionId是否相同, 不同的話清除session並且重定向到Login
                    if (UserSessionId != HttpContext.Current.Session.SessionID)
                    {
                        filterContext.Controller.TempData["KickOutMessage"] = "您的帳號已被他人踢出";
                        filterContext.Result = new RedirectResult("/Login/Index");
                        base.OnActionExecuting(filterContext);
                        return;
                    }

                    //隨時更新角色權限
                    if (userSessionModel.UserPermission != UserPermissions)
                    {
                        filterContext.Controller.TempData["KickOutMessage"] = "您的權限已被更動, 請重新登入";
                        filterContext.Result = new RedirectResult("/Login/Index");
                        base.OnActionExecuting(filterContext);
                        return;
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