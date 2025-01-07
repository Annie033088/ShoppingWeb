using NLog;
using Pashamao.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Filters
{
    public class UserRoleAuthFilter : ActionFilterAttribute
    {
        private readonly long requiredPermissions;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="role"></param>
        public UserRoleAuthFilter(UserPermission requiredPermission)
        {
            //傳進來是需要的權限相加(並非實質定義)
            requiredPermissions = (long)requiredPermission;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                UserSessionModel userSession = HttpContext.Current.Session["UserSession"] as UserSessionModel;

                if (userSession == null)
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                    return;
                }
                else if ((userSession.UserPermission & requiredPermissions) == 0)
                {
                    filterContext.Controller.TempData["NoPermissionMessage"] = "您無此權限";
                    filterContext.Result = new RedirectResult("/MainHome/Index");
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