﻿using CMS_DTO.CMSHome;
using CMS_Shared;
using CMS_Shared.Utilities;
using CMS_Web.Web.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;

namespace CMS_Web.Areas.Admin.Controllers
{
    [NuAuth]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("CMSAccount", new { area = "Admin" });

                FormsAuthentication.SignOut();
                Session.Remove("User");
                HttpCookie currentUserCookie = HttpContext.Request.Cookies["UserCookie"];
                HttpContext.Response.Cookies.Remove("UserCookie");
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                HttpContext.Response.SetCookie(currentUserCookie);
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Logout Error: ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }
    }
}