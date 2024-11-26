using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Filters
{
    public class UserRoleAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting( ActionExecutingContext filterContext )
        {
            SessionModel userModel = HttpContext.Current.Session["UserSession"] as SessionModel;

            if (userModel == null)
            {
                filterContext.Result = new RedirectResult ( "/Login/Index" );
            } else
            { 
                
            }
                base.OnActionExecuting ( filterContext );
        }
    }
}