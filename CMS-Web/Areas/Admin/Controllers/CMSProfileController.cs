using CMS_Web.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class CMSProfileController : Controller
    {
        // GET: Admin/CMSProfile
        [NuAuth]
        public ActionResult Index()
        {
            return View();
        }
    }
}