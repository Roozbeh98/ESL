using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ESL.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }

        protected void Application_AuthorizeRequest()
        {
            var AuthCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (AuthCookie != null)
            {
                FormsAuthenticationTicket Ticket = null;
                try
                {
                    Ticket = FormsAuthentication.Decrypt(AuthCookie.Value);
                }
                catch
                {

                }
                if (Ticket != null)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Ticket.Name), Ticket.UserData.Split(','));
                    HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(Ticket.Name), Ticket.UserData.Split(','));

                }

            }
        }
    }
}