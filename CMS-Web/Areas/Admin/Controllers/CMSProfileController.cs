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
        public ActionResult Index()
        {
            return View();
        }
    }
}