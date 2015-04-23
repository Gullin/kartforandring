using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace kartforandring
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

            // Routing för Web Api
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // Routing för Web Forms
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}