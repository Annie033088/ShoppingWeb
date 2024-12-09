using NLog;
using System;
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
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            logger.Info("Application Start");

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            if (exception != null)
            {
                logger.Error(exception);
                if ( Session["UserVisitState"] != null)
                {
                    if (Session["UserVisitState"].ToString() == "User") Response.Redirect("/MainHome/Index");
                }
                Response.Redirect("/Login/Index");
                Server.ClearError();
            }
        }

        protected void Application_End()
        {
            logger.Info("Application End");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["UserVisitState"] = "Guest";
        }
    }
}
