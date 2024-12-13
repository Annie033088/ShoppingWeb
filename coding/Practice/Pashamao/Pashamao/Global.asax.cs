using NLog;
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Diagnostics;

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

            string source = "PashaMaoSource";
            string logName = "PashaMao";

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, logName);
            }

            EventLog eventLog = new EventLog();
            eventLog.Source = source;

            eventLog.WriteEntry(exception.ToString(), EventLogEntryType.Error);
            
            if (exception != null)
            {
                logger.Error(exception);

                if ( Session["UserSession"] != null) Response.Redirect("/MainHome/Index");

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
        }
    }
}
