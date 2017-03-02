using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Maestrano;
using log4net;

namespace MnoDemoApp.Controllers
{


    public class MaestranoController : Controller
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(MaestranoController));

        /// <summary> /maestrano/metadata/{marketplace}</summary>
        /// <param name="marketplace"> marketplace for which the saml is being done
        [HttpGet]
        public ActionResult Metadata(string marketplace)
        {
            string result = null;
            var request = System.Web.HttpContext.Current.Request;
            var preset = MnoHelper.With(marketplace);
            if (preset.Authenticate(request))
            {
                result = preset.ToMetadata().ToString();
            }
            else
            {
                result = "Can't authenticate for marketplace: " + marketplace;
            }

            return Content(result, "application/json");
        }

        /// <summary> /maestrano/init/?marketplace={marketplace}</summary>
        [HttpGet]
        public ActionResult Init(string marketplace)
        {
            var request = System.Web.HttpContext.Current.Request;

            var ssoUrl = MnoHelper.With(marketplace).Sso.BuildRequest(request.QueryString).RedirectUrl();
            return Redirect(ssoUrl);
        }

        /// <summary> /maetrano/consume/?marketplace={marketplace}</summary>
        /// <param name="marketplace"> marketplace for which the saml is being done
        [HttpPost]
        public ActionResult Consume(string marketplace)
        {
            var request = System.Web.HttpContext.Current.Request;
            //Retrieving the Maestrano configuration preset from the marketplace id
            var preset = MnoHelper.With(marketplace);
            // Get SAML response, build maestrano user and group objects
            var samlResp = preset.Sso.BuildResponse(request.Params["SAMLResponse"]);

            // Check response validity
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
                session["email"] = mnoUser.Email;
                session["id"] = mnoUser.Uid;

                // Flag user as logged in
                session["loggedIn"] = true;

                session["marketplace"] = marketplace;

                // Set Maestrano session - used for Single Logout
                preset.Sso.SetSession(session, mnoUser);
                return Redirect("/");
            }
            else
            {
                logger.Error("Invalid SAML Response: " + Request.Params["SAMLResponse"]);
                return Content("Invalid SAML Response");
            }
        }

        [HttpDelete]
        public ActionResult Groupdeleted(string marketplace, string groupid)
        {
            logger.Debug("Groupdeleted webhook received for " + marketplace + " , " + groupid);
            return Content("OK");
        }
    }
}
