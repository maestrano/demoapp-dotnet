using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Maestrano;

namespace MnoDemoApp.Controllers
{
    public class BillsController : Controller
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

            var filter = new System.Collections.Specialized.NameValueCollection { {"group_id",(string) Session["groupId"]} };
            ViewBag.Bills = Maestrano.Account.Bill.All(filter);

            return View();
        }
    }
}
