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
                var mnoSession = new Maestrano.Sso.Session(session);

                if (!mnoSession.IsValid())
                {
                    Response.Redirect(MnoHelper.Sso.InitUrl());
                }
            }

            

            return View();
        }

        public ActionResult Logout()
        {
            var session = System.Web.HttpContext.Current.Session;
            session.Clear();

            return Redirect("/");
        }
    }
}
