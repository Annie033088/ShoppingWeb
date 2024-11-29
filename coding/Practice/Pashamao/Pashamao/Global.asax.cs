using NLog;
using Pashamao.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Pashamao
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            //可以註冊區域 方便管理不同分區
            //AreaRegistration.RegisterAllAreas (); 

            //全局過濾器 在每次請求都會被調用
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //註冊路由規則
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //註冊bundle
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            logger.Info("Application Start");

        }

        protected void Application_Error(Exception error)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                logger.Error(error);
                Response.Redirect("/Views/Login/Index.cshtml");
                Server.ClearError();
            }
        }

        protected void Application_End()
        {
            logger.Info("Application End");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["UserName"] = "Guest";
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session.Clear();          // 清除所有的 session 資料
            Session.Abandon();        // 廢棄當前 session
        }

    }
}
