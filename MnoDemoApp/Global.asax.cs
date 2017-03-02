using log4net;
using log4net.Config;
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
        private static readonly ILog logger = LogManager.GetLogger(typeof(MvcApplication));


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AuthConfig.RegisterAuth();

            // Log4Net Configuration
            XmlConfigurator.Configure();
            // Adding TSL12 Security Protocol
            // Maestrano does not support SSL3 protocol as it is insecure
            // Maestrano supports TLS 1.2
            // https://blog.mozilla.org/security/2014/10/14/the-poodle-attack-and-the-end-of-ssl-3-0/
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            logger.Debug("Maestrano autoconfiguring...");

            //Auto Configure Maestrano using Environment variables
            Maestrano.MnoHelper.AutoConfigure();

            //Auto Configure Maestrano directly
            //var apiKey = "[YOUR-API-KEY]";
            //var apiSecret = "[YOUR-API-SECRET]";
            //Maestrano.MnoHelper.AutoConfigure("https://developer.maestrano.com", "/api/config/v1", apiKey, apiSecret);

            logger.Debug("Maestrano configured with: " + String.Join(",", Maestrano.MnoHelper.Presets().Keys));
        }
    }
}