using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            Database.SetInitializer(new SamplaData());
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}