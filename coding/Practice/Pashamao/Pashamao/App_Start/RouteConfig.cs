﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Pashamao
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "MainHome", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "noUrl",
                url: "{*url}",
                defaults: new { controller = "MainHome", action = "Index" }
            );
        }
    }
}
