using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;


namespace ESL
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
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
