using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Kartforandring
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("",
                                "lageskontroll",
                                "~/lageskontroll.aspx");
            routes.MapPageRoute("",
                                "kartforandring",
                                "~/kartforandring.aspx");
            routes.MapPageRoute("",
                                "kontrollpanel",
                                "~/dashboard.aspx");
        }
    }
}