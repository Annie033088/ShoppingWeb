using Pashamao.Models;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Filters
{
    public class UserRoleAuthFilter : ActionFilterAttribute
    {
        private readonly long requiredPermissions;

        /// <summary>
        /// 權限限制
        /// </summary>
        /// <param name="role"></param>
        public UserRoleAuthFilter(UserPermission requiredPermission) { requiredPermissions = (long)requiredPermission; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserSessionModel userSession = HttpContext.Current.Session["UserSession"] as UserSessionModel;

            //利用位元運算, 沒有符合就返回
            if ((userSession.UserPermission & requiredPermissions) == 0)
            {
                filterContext.Result = new RedirectResult("/MainHome/Index");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}