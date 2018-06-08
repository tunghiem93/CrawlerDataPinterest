using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_DTO.CMSSession;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class BaseController : HQController
    {
        // GET: Admin/Base
        public string List { get; set; }
        // GET: Base
        //public UserSession CurrentUser
        //{
        //    get
        //    {
        //        if (System.Web.HttpContext.Current.Session["User"] != null)
        //            return (UserSession)System.Web.HttpContext.Current.Session["User"];
        //        else
        //            return new UserSession();
        //    }
        //}

        public BaseController()
        {
            var user = System.Web.HttpContext.Current.Session["User"] as UserSession;
        }
    }
}