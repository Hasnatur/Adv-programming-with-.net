using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zero_hunger.Authorization
{
    public class emAccess : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["user"] != null && httpContext.Session["type"].ToString().Equals("employee");
        }
    }
}