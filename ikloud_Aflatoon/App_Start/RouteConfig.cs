﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ikloud_Aflatoon
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

    //        routes.MapRoute(
    //    name: "Unauthorized",
    //    url: "{controller}/{action}/{id}",
    //    defaults: new
    //    {
    //        controller = "Error",
    //        action = "NotFound",
    //        id = UrlParameter.Optional
    //    }
    //);
        }
    }
}