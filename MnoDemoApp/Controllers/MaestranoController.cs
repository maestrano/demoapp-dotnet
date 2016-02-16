using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Maestrano;

namespace MnoDemoApp.Controllers
{
    public class MaestranoController : Controller
    {
        public ActionResult Metadata()
        {
            string result  = null;
            var request = System.Web.HttpContext.Current.Request;

            if (MnoHelper.Authenticate(request))
            {
                result = MnoHelper.ToMetadata().ToString();
            }
            else
            {
                result = "Failed";
            }
            
            return Content(result, "application/json");
        }

        public ActionResult Init()
        {
            var request = System.Web.HttpContext.Current.Request;

            var ssoUrl = MnoHelper.Sso.BuildRequest(request.QueryString).RedirectUrl();
            return Redirect(ssoUrl);
        }

        public ActionResult Consume()
        {
            var request = System.Web.HttpContext.Current.Request;

            // Get SAML response, build maestrano user and group objects
            var samlResp = MnoHelper.Sso.BuildResponse(request.Params["SAMLResponse"]);

            if (samlResp.IsValid())
            {
                var mnoUser = new Maestrano.Sso.User(samlResp);
                var mnoGroup = new Maestrano.Sso.Group(samlResp);
                var session = System.Web.HttpContext.Current.Session;

                // At this step we should link the user and group to actual
                // models in our application. This application does not have any
                // model so we just store things in session to simulate persistence
                session["firstName"] = mnoUser.FirstName;
                session["lastName"] = mnoUser.LastName;
                session["groupName"] = mnoGroup.Name;
                session["groupId"] = mnoGroup.Uid;

                // Important - toId() and toEmail() have different behaviour compared to
                // getId() and getEmail(). In you maestrano configuration file, if your sso > creation_mode 
                // is set to 'real' then toId() and toEmail() return the actual id and email of the user which
                // are only unique across users.
                // If you chose 'virtual' then toId() and toEmail() will return a virtual (or composite) attribute
                // which is truly unique across users and groups
                session["email"] = mnoUser.ToEmail();
                session["id"] = mnoUser.ToUid();

                // Flag user as logged in
                session["loggedIn"] = true;


                // Set Maestrano session - used for Single Logout
                MnoHelper.Sso.SetSession(session, mnoUser);
            }
            

            return Redirect("/") ;
        }


    }
}
