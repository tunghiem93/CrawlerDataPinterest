using CMS_DTO.CMSSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMS_Web.Web.App_Start
{
    public class NuAuthAttribute : AuthorizeAttribute
    {
        private UserSession _CurrentUser;

        private string ActionType;

        private string Controller;
        private string Action;

        private List<string> _Views = new List<string> { "index", "default", "view", "detail", "get", "load", "filter", "search", "apply" };

        /*Factory*/

        public NuAuthAttribute()
        {
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["User"] == null)
                _CurrentUser = new UserSession();
            else
                _CurrentUser = (UserSession)HttpContext.Current.Session["User"];
                        
            //Alias Controller //Action
            Controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            Action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString().ToLower();

            bool isViewAction = _Views.Any(s => Action.Contains(s));
            ActionType = isViewAction ? "View" : "Action";

            return IsPermission();
        }

        protected bool IsPermission()
        {
            try
            {
                // If user not logged in, require login
                if (!_CurrentUser.IsAuthenticated)
                    return false;
                else
                {
                    if (_CurrentUser.IsSuperAdmin || Controller.ToLower().Equals("home"))
                    {
                        return true;
                    }
                    return true;
                }

            }
            catch (Exception e)
            {
                string error = e.ToString();
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!_CurrentUser.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "CMSAccount",
                            action = "Index",
                            area = "Admin",
                            isAjax = filterContext.HttpContext.Request.IsAjaxRequest(),
                            returnUrl = filterContext.HttpContext.Request.Url.ToString().Replace("/Logout", "")
                        })
                    );
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Error",
                            action = "Unauthorised",
                            area = string.Empty,
                        })
                    );
            }
        }
    }
}