using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Filters
{
    public class UserKickOutFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessionModel userModel = HttpContext.Current.Session["UserSession"] as SessionModel;

            if (userModel == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
            else
            {
                //取得DB的sessionId
                UserRepository userRepository = new UserRepository();
                string DBSessionId = userRepository.GetSessionId(userModel);

                //判斷現sessionId與資料庫的sessionId是否相同, 不同的話清除session並且重定向到Login
                if (DBSessionId != HttpContext.Current.Session.SessionID)
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.Abandon();
                    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-1);
                    filterContext.Result = new RedirectResult("/Login/Index");
                }

            }

            base.OnActionExecuting(filterContext);
        }
    }
}