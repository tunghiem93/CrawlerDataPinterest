using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMS_Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                 "Default", // Route name
                 "{controller}/{action}/{id}", // URL with parameters
                 new { area = "Admin", controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                 null,
                 new[] { "CMS_Web.Areas.Admin.Controllers" }
             ).DataTokens.Add("area", "Admin");
        }
    }
}
