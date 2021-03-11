using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http.Extensions;

namespace Foundation.Models
{
    public class CheckSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int? ID = filterContext.HttpContext.Session.GetInt32("usrId");

            if (ID == null)
            {
                filterContext.Result =
                        new RedirectToRouteResult(new RouteValueDictionary
                                {
                              { "action", "Logout" },
                            { "controller", "User" }
                                 });
                return;
            }
        }
    }

    public class SessionCheckForAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int? ID = filterContext.HttpContext.Session.GetInt32("usrId");
            if (ID == null)
            {
                int? Role = filterContext.HttpContext.Session.GetInt32("roleId");
                if (Role != 1)
                {
                    filterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary
                        {
                              { "action", "Logout" },
                            { "controller", "User" }
                         });
                    return;
                }
            }
        }
    }



}