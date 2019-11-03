using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Models.Attribute
{
    public class CustomAuthorizeAttribute:AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Areas/Dashboard/Views/Account/Login.cshtml",
            };
        }
    }
}