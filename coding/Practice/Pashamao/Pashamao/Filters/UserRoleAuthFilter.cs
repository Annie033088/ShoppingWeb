﻿using Pashamao.Models;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Filters
{
    public class UserRoleAuthFilter : ActionFilterAttribute
    {
        private readonly UserRole Role;

        /// <summary>
        /// 權限限制
        /// </summary>
        /// <param name="role"></param>
        public UserRoleAuthFilter(UserRole role) { Role = role; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserSessionModel userModel = HttpContext.Current.Session["UserSession"] as UserSessionModel;
            int roleId = (int)Role;

            //如果權限比較小, 返回當前當前主頁   (設定是id越小, 權限越大)
            if (userModel.RoleId > roleId)
            {
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                string redirectUrl = "/" + controllerName + "/" + "Index";
                filterContext.Result = new RedirectResult(redirectUrl);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}