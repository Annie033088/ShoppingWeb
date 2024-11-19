using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Pashamao
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();

        protected void Application_Start()
        {
            //�i�H���U�ϰ� ��K�޲z���P����
            //AreaRegistration.RegisterAllAreas ();

            //�����L�o�� �b�C���ШD���|�Q�ե�
            FilterConfig.RegisterGlobalFilters ( GlobalFilters.Filters );

            //���U���ѳW�h
            RouteConfig.RegisterRoutes ( RouteTable.Routes );

            //���Ubundle
            BundleConfig.RegisterBundles ( BundleTable.Bundles );
            logger.Info ( "Application Start" );
        }

        protected void Application_Error( Exception error )
        {
            logger.Error( error );
        }

       protected void Application_End()
        {
            logger.Info ( "Application End");
        }

    }
}
