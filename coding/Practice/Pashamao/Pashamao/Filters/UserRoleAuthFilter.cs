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
        /// 權限限制
        /// </summary>
        /// <param name="role"></param>
        public UserRoleAuthFilter(UserPermission requiredPermission) { requiredPermissions = (long)requiredPermission; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                UserSessionModel userSession = HttpContext.Current.Session["UserSession"] as UserSessionModel;

                //利用位元運算, 沒有符合就返回
                if ((userSession.UserPermission & requiredPermissions) == 0)
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