using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NAGGLE.Web.Helper
{
    public class AuthorizeUser : AuthorizeAttribute
    {
        private bool _isAuthorized;

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            _isAuthorized = base.AuthorizeCore(httpContext);
            return _isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated == false)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "SessionTimeOut" }));
                }
                else if (HttpContext.Current.User.Identity.Name == string.Empty)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                }
            }
            else if (!_isAuthorized)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
            }

        }
    }
}