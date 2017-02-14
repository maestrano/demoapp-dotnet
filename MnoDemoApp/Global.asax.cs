using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MnoDemoApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //http://stackoverflow.com/questions/10822509/the-request-was-aborted-could-not-create-ssl-tls-secure-channel
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls12
                | SecurityProtocolType.Ssl3;
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AuthConfig.RegisterAuth();
            //Auto Configure Maestrano using Environment variables
            Maestrano.MnoHelper.AutoConfigure();
            //Auto Configure Maestrano directly
            //var apiKey = "[YOUR-API-KEY]";
            //var apiSecret = "[YOUR-API-SECRET]";
            //Maestrano.MnoHelper.AutoConfigure("https://developer-uat.maestrano.io", "/api/config/v1", apiKey, apiSecret);


        }
    }
}