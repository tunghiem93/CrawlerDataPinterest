using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Clients.Controllers
{
    public class NotFoundController : Controller
    {
        // GET: Clients/NotFound
        public ActionResult Index()
        {
            return View();
        }
    }
}