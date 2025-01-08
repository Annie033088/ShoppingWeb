using Microsoft.Ajax.Utilities;
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
                    //使用者未登入卻輸入登入後的url
                    filterContext.Result = new RedirectResult("/LoginIndex");
                    return;
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
                        //禁用的errorCode
                        int errorCode = 3;
                        filterContext.Result = new JsonResult()
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new { errorCode }
                        };
                        clearTheUser();
                        return;
                    }

                    //判斷現sessionId與資料庫的sessionId是否相同, 不同的話清除session並且重定向到Login
                    if (UserSessionId != HttpContext.Current.Session.SessionID)
                    {
                        //後踢前的errorCode
                        int errorCode = 2;
                        filterContext.Result = new JsonResult()
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new { errorCode }
                        };
                        clearTheUser();
                        return;
                    }

                    //隨時更新角色權限
                    if (userSessionModel.UserPermission != UserPermissions)
                    {
                        //權限更動的errorCode
                        int errorCode = 4;
                        filterContext.Result = new JsonResult()
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new { errorCode }
                        };
                        clearTheUser();
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

        private void clearTheUser()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-1);
            return;
        }
    }
}