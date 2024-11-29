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
            //�i�H���U�ϰ� ��K�޲z���P����
            //AreaRegistration.RegisterAllAreas (); 

            //�����L�o�� �b�C���ШD���|�Q�ե�
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //���U���ѳW�h
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //���Ubundle
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
            Session.Clear();          // �M���Ҧ��� session ���
            Session.Abandon();        // �o���e session
        }

    }
}
