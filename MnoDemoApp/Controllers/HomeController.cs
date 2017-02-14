using Maestrano;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MnoDemoApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            // Check session is still valid (only if user logged in)
            var session = System.Web.HttpContext.Current.Session;
            if (session["loggedIn"] != null)
            {
                string marketplace = (String) session["marketplace"];
                var preset = MnoHelper.With(marketplace);
                var mnoSession = new Maestrano.Sso.Session(preset, session);

                if (!mnoSession.IsValid())
                {
                    Response.Redirect(MnoHelper.With(marketplace).Sso.InitUrl());
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            String url = "/";
            var session = System.Web.HttpContext.Current.Session;
            if (session["loggedIn"] != null)
            {
                string marketplace = (String)session["marketplace"];
                var preset = MnoHelper.With(marketplace);
                var mnoSession = new Maestrano.Sso.Session(preset, session);
                url = mnoSession.LogoutUrl();
            }
            session.Clear();
            return Redirect(url);
        }
    }
}
