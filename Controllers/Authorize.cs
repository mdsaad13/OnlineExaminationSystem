using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.HttpContext.Session["AdminID"] == null)
            {
                context.Result = new RedirectResult("/AdminLogin");
            }
        }
    }

    public class StudentAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.HttpContext.Session["StudentID"] == null)
            {
                context.Result = new RedirectResult("/login");
            }
        }
    }

    public class IsAuthorized : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.HttpContext.Session["AdminID"] != null)
            {
                context.Result = new RedirectResult("/Admin");
            }
            else if (context.HttpContext.Session["StudentID"] != null)
            {
                context.Result = new RedirectResult("/Student");
            }
        }
    }
}