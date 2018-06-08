using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class CMSCustomersController : Controller
    {
        // GET: Admin/CMSCustomers
        public ActionResult Index()
        {
            return View();
        }
    }
}